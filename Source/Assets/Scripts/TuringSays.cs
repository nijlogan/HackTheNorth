using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TuringSays : MonoBehaviour
{
    public TextMeshProUGUI TimerDisplay;
    public TextMeshProUGUI Progress;
    public TextMeshProUGUI Divider;
    public TMP_InputField Result;

    public GameObject ReadHead;
    public SpriteRenderer[] ReadEntries;
    public TextMeshProUGUI[] ReadEntryTexts;

    public SpriteRenderer Arrowhead;

    //Pre-Game stuff
    public GameObject Tutorial;
    public GameObject ReadyStart;
    public GameObject Game;
    public TextMeshProUGUI counter;

    //Tutorial Screen
    public GameObject speechA;
    public GameObject StephA;
    public GameObject picA;
    public GameObject speechB;
    public GameObject StephB;
    public GameObject picB;
    public GameObject speechC;
    public GameObject StephC;
    public GameObject StartButton;

    //Scoreboard stuff
    public GameObject ScoreBoard;
    public TextMeshProUGUI scoreBoard;
    public static int scoreNum = 1;
    public Animator transition;

    public AudioSource SE;
    //GUI Sound Effects
    public AudioClip correct;
    public AudioClip mistake;
    public AudioClip confirm;
    public AudioClip three;
    public AudioClip two;
    public AudioClip one;
    public AudioClip start;
    public AudioClip timeup;

    //Turing Sound Effects
    public AudioClip turingtick;

    public int numCorrect = 0;
    public int numAnswered = 0;

    private int sequenceLength = 2;
    private int sequenceDirection = 1;

    private int turingTime = 11;

    private string inputSequence = "";

    private bool inProgress = true;
    private bool answerComplete = false;
    private bool peeking = false;
    private bool timerActive = false;

    private void Start()
    {
        StartCoroutine("TutorialStart");
    }

    public void skipTutorial()
    {
        speechA.SetActive(false);
        StephA.SetActive(false);
        picA.SetActive(false);
        speechB.SetActive(false);
        StephB.SetActive(false);
        picB.SetActive(false);
        StopCoroutine("TutorialStart");
        startGame();
    }

    IEnumerator TutorialStart()
    {
        yield return new WaitForSeconds(0.2f);

        speechA.SetActive(true);
        StephA.SetActive(true);
        picA.SetActive(true);

        yield return new WaitForSeconds(7f);

        speechA.SetActive(false);
        StephA.SetActive(false);
        picA.SetActive(false);
        speechB.SetActive(true);
        StephB.SetActive(true);
        picB.SetActive(true);

        yield return new WaitForSeconds(7f);

        speechB.SetActive(false);
        StephB.SetActive(false);
        picB.SetActive(false);
        speechC.SetActive(true);
        StephC.SetActive(true);

        yield return new WaitForSeconds(5f);

        StartButton.SetActive(true);

        yield return null;
    }

    public void startGame()
    {
        StephC.SetActive(false);
        SE.PlayOneShot(confirm);
        StartCoroutine("getReady");
    }

    IEnumerator getReady()
    {
        Tutorial.SetActive(false);
        ReadyStart.SetActive(true);

        counter.text = "Ready?";
        yield return new WaitForSeconds(1f);
        counter.text = "3";
        SE.PlayOneShot(three);
        yield return new WaitForSeconds(1f);
        counter.text = "2";
        SE.PlayOneShot(two);
        yield return new WaitForSeconds(1f);
        counter.text = "1";
        SE.PlayOneShot(one);
        yield return new WaitForSeconds(1f);
        counter.text = "Start!";
        SE.PlayOneShot(start);
        yield return new WaitForSeconds(0.5f);

        ReadyStart.SetActive(false);
        Game.SetActive(true);

        Divider.text = "___";

        ReadHead.transform.position = ReadEntries[0].transform.position;

        PrimeSequence();
        ManageVisibility();

        Result.Select();

        StartCoroutine(Peek());
        StartCoroutine(ProvideQuestion());

        yield return null;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SE.PlayOneShot(turingtick);
        }

        if ((Input.GetKeyDown(KeyCode.Return) || answerComplete) && !inProgress)
        {
            string sequenceAnswer = "";

            for (int i = 0; i < sequenceLength; i++)
            {
                sequenceAnswer += ReadEntryTexts[i].text;
            }

            if (sequenceDirection == 0)
            {
                sequenceAnswer = Reverse(sequenceAnswer);
            }

            if (inputSequence == sequenceAnswer)
            {
                SE.PlayOneShot(correct);
                numCorrect++;
            } else
            {
                SE.PlayOneShot(mistake);
            }

            numAnswered++;

            Progress.text = numCorrect + "/" + numAnswered;

            Result.ActivateInputField();
            Result.text = "";

            if (!inProgress)
            {
                if (numAnswered % 12 == 0)
                {
                    PrimeSequence();
                }

                sequenceLength = Mathf.Min(7, Mathf.CeilToInt(((numAnswered % 12) + 3) / 2.0f));
                sequenceDirection = 1 - sequenceDirection;
                StartCoroutine(Peek());
                StartCoroutine(ProvideQuestion());
            }
        }
    }

    public void PrimeSequence()
    {
        for (int i = 0; i < 7; i++)
        {
            ReadEntryTexts[i].text = UnityEngine.Random.Range(0, 10) + "";
        }
    }

    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public IEnumerator ProvideQuestion()
    {
        inProgress = true;
        answerComplete = false;

        inputSequence = "";

        while (peeking)
        {
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(Countdown());

        int iStart = (sequenceLength - 1) * (1 - sequenceDirection);
        int iEnd = sequenceLength * sequenceDirection - 1 * (1 - sequenceDirection);
        int iChange = -1 + 2 * sequenceDirection;

        for (int i = iStart; i != iEnd; i += iChange)
        {
            Result.ActivateInputField();
            Result.text = "";

            StartCoroutine(MoveReadHead(ReadEntries[i]));

            yield return new WaitUntil(() => !Input.GetKeyDown(KeyCode.Return));

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

            inputSequence += Result.text;
        }

        timerActive = false;
        TimerDisplay.text = "--";

        inProgress = false;
        answerComplete = true;
    }

    public IEnumerator MoveReadHead(SpriteRenderer toObj)
    {
        Vector2 vecA = ReadHead.transform.position;
        Vector2 vecB = toObj.transform.position;

        for (int i = 1; i <= 10; i++)
        {
            ReadHead.transform.position = Vector2.Lerp(vecA, vecB, i/10.0f);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ManageVisibility()
    {
        for (int i = 0; i < 7; i++)
        {
            ReadEntries[i].color = Color.black;
        }
    }

    public IEnumerator Peek()
    {
        peeking = true;

        Quaternion theRotation = transform.localRotation;
        theRotation.z = 180 * (1 - sequenceDirection);
        Arrowhead.transform.rotation = theRotation;

        for (int i = 0; i < sequenceLength; i++)
        {
            ReadEntries[i].color = Color.white;
        }

        yield return new WaitForSeconds(1);

        for (int i = 0; i < 7; i++)
        {
            ReadEntries[i].color = Color.black;
        }

        peeking = false;
    }

    public IEnumerator Countdown()
    {
        timerActive = true;
        turingTime = 10;

        while (timerActive && 0 < turingTime)
        {
            TimerDisplay.text = turingTime.ToString();
            turingTime--;

            yield return new WaitForSeconds(1.0f);
        }

        if (timerActive)
        {
            endGame();
        }

    }

    public void endGame()
    {
        SE.PlayOneShot(timeup);
        StartCoroutine(Timesup());
    }

    IEnumerator Timesup()
    {
        Game.SetActive(false);
        ReadyStart.SetActive(true);

        counter.text = "Time's up!";

        yield return new WaitForSeconds(2f);

        goToScores();

        yield return null;

    }

    void goToScores()
    {
        ScoreKeeper._instance.addScore(scoreNum.ToString(), numCorrect);
        ScoreBoard.SetActive(true);
        List<int> scores = new List<int>();

        for (int i = 1; i <= scoreNum; i++)
        {
            int score = PlayerPrefs.GetInt(i.ToString());
            scores.Add(score);
        }
        scoreNum++;

        scores.Sort();

        for (int i = 0; i < scores.Count; i++)
        {
            if (i > 10)
            {
                break;
            }

            scoreBoard.text += i + 1 + ".            " + scores[i] + "\n";
        }

        Result.ReleaseSelection();
        Result.DeactivateInputField();
    }

    public void ReturnToGameSelect()
    {
        transition.SetTrigger("Start");
        SE.PlayOneShot(confirm);
        scoreBoard.text = "";
        SceneManager.LoadScene(1);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Arithmetic : MonoBehaviour
{
    //Game Setup
    public GameObject Tutorial;
    public GameObject Game;
    public GameObject ReadyStart;
    public TextMeshProUGUI counter;

    //Game display
    public TextMeshProUGUI TimerDisplay;
    public TextMeshProUGUI Progress;
    public TextMeshProUGUI Query;
    public TextMeshProUGUI Divider;
    public TMP_InputField Result;

    //Tutorial Screen
    public GameObject speech1;
    public GameObject StephA;
    public GameObject speech2;
    public GameObject StephB;
    public GameObject speech3;
    public GameObject StephC;
    public GameObject StartButton;
    public GameObject numbers;
    public GameObject timer;
    public GameObject increase;

    //ScoreBoard stuff
    public GameObject ScoreBoard;
    public TextMeshProUGUI scoreBoard;
    public static int scoreNum = 1;
    public Animator transition;

    //Audio Stuff
    public AudioSource SE;
    public AudioClip correct;
    public AudioClip mistake;
    public AudioClip confirm;
    public AudioClip three;
    public AudioClip two;
    public AudioClip one;
    public AudioClip start;
    public AudioClip timeup;

    public int numCorrect = 0;
    public int numAnswered = 0;

    private int countdownTime = 60;

    private int arithmeticAnswer = 0;

    // private const int ADDITION = 0;
    private const int SUBTRACTION = 1;
    private const int MULTIPLICATION = 2;
    private const int DIVISION = 3;

    // Start is called before the first frame update
    void Start()
    {
        Query.text = "";
        Divider.text = "________";

        StartCoroutine("TutorialStart");
    }

    IEnumerator TutorialStart()
    {
        yield return new WaitForSeconds(0.2f);

        speech1.SetActive(true);
        StephA.SetActive(true);
        numbers.SetActive(true);

        yield return new WaitForSeconds(5f);

        speech1.SetActive(false);
        StephA.SetActive(false);
        speech2.SetActive(true);
        StephB.SetActive(true);
        timer.SetActive(true);

        yield return new WaitForSeconds(5f);

        speech2.SetActive(false);
        StephB.SetActive(false);
        speech3.SetActive(true);
        StephC.SetActive(true);
        timer.SetActive(false);
        numbers.SetActive(false);
        increase.SetActive(true);

        yield return new WaitForSeconds(4f);

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

        Result.Select();

        StartCoroutine("Countdown");

        ProvideQuestion();

        yield return null;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Result.text == "")
            {
                Result.text = "0";
            }

            int answer = int.Parse(Result.text);

            if (answer == arithmeticAnswer)
            {
                SE.PlayOneShot(correct);
                numCorrect++;
            }
            else
            {
                SE.PlayOneShot(mistake);
            }
            

            numAnswered++;

            Progress.text = numCorrect + "/" + numAnswered;

            Result.ActivateInputField();
            Result.text = "";

            ProvideQuestion();
        }
    }

    public void ProvideQuestion()
    {
        // addition, subtraction, multiplication, division
        int equationType = Random.Range(0, Mathf.Min(4, 1 + Mathf.FloorToInt(numCorrect/5)));

        int num1, num2;

        switch (equationType)
        {
            case (SUBTRACTION):
                num1 = Random.Range(5, 10) + numCorrect;
                num2 = Random.Range(1, 5) + numCorrect;
                Query.text = num1 + "\n - " + num2;
                arithmeticAnswer = num1 - num2;
                break;
            case (MULTIPLICATION):
                num1 = Random.Range(2, 5) + Mathf.FloorToInt((numCorrect - 10) / 2);
                num2 = Random.Range(2, 5) + Mathf.FloorToInt((numCorrect - 10) / 2);
                Query.text = num1 + "\n x " + num2;
                arithmeticAnswer = num1 * num2;
                break;
            case (DIVISION):
                num2 = Random.Range(2, 5) + Mathf.FloorToInt((numCorrect - 15) / 2);
                num1 = num2 * Random.Range(2, 5) + Mathf.FloorToInt((numCorrect - 15) / 2);
                Query.text = num1 + "\n ÷ " + num2;
                arithmeticAnswer = num1 / num2;
                break;
            default:
                num1 = Random.Range(1, 8) + numCorrect;
                num2 = Random.Range(1, 8) + numCorrect;
                Query.text = num1 + "\n + " + num2;
                arithmeticAnswer = num1 + num2;
                break;
        }
    }

    IEnumerator Countdown()
    {
        while (0 < countdownTime)
        {
            countdownTime--;
            TimerDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1.0f);
        }

        endGame();
        yield return null;
    }

    void endGame()
    {
        SE.PlayOneShot(timeup);
        StartCoroutine("Timesup");
    }

    IEnumerator Timesup()
    {
        Game.SetActive(false);
        ReadyStart.SetActive(true);

        counter.text = "Times up!";

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

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseGame : MonoBehaviour
{
    private Hiders housePeople;
    private Hiders treePeople;
    private Hiders rockPeople;

    public GameObject[] Spawners;

    public GameObject[] Covers;

    public GameObject Person;

    //Game Setup
    public GameObject Tutorial;
    public GameObject Game;
    public GameObject ReadyStart;
    public TextMeshProUGUI counter;

    //Tutorial Screen
    public GameObject speech1;
    public GameObject StephA;
    public GameObject speech2;
    public GameObject StephB;
    public GameObject speech3;
    public GameObject StephC;
    public GameObject StartButton;
    public GameObject picA;
    public GameObject picB;

    public GameObject house;
    public GameObject rock;
    public GameObject tree;

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

    public TextMeshProUGUI Query;
    public TextMeshProUGUI Progress;

    public TMP_InputField Result;

    private string[] coverTexts = { "house", "tree", "rock" };

    private int difficulty = 0;

    private bool spawning = false;
    private int activePeople = 0;
    private bool questioning = false;

    private int coverAnswer = 0;

    private int numCorrect = 0;
    private int numAnswered = 0;

    private void Start()
    {
        StartCoroutine("TutorialStart");
    }

    public void skipTutorial()
    {
        speech1.SetActive(false);
        StephA.SetActive(false);
        picA.SetActive(false);
        speech2.SetActive(false);
        StephB.SetActive(false);
        StephC.SetActive(false);
        StopCoroutine("TutorialStart");
        startGame();
    }

    IEnumerator TutorialStart()
    {
        yield return new WaitForSeconds(0.2f);

        speech1.SetActive(true);
        StephA.SetActive(true);
        picA.SetActive(true);

        yield return new WaitForSeconds(8f);

        speech1.SetActive(false);
        StephA.SetActive(false);
        picA.SetActive(false);
        speech2.SetActive(true);
        StephB.SetActive(true);
        picB.SetActive(true);

        yield return new WaitForSeconds(8f);

        speech2.SetActive(false);
        StephB.SetActive(false);
        speech3.SetActive(true);
        StephC.SetActive(true);
        picB.SetActive(false);

        yield return new WaitForSeconds(6f);

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
        house.SetActive(true);
        rock.SetActive(true);
        tree.SetActive(true);
        housePeople = GameObject.FindGameObjectWithTag("House").GetComponent<Hiders>();
        treePeople = GameObject.FindGameObjectWithTag("Tree").GetComponent<Hiders>();
        rockPeople = GameObject.FindGameObjectWithTag("Rock").GetComponent<Hiders>();

        ClearPeople();
        StartCoroutine(SpawnPeople());
        StartCoroutine(Questionnaire());

        yield return null;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !questioning)
        {
            if (Result.text == "")
            {
                Result.text = "-1";
            }

            int answer = int.Parse(Result.text);

            int[] peopleAnswers = { housePeople.getPeople(), treePeople.getPeople(), rockPeople.getPeople() };

            if (answer == peopleAnswers[coverAnswer])
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

            ClearPeople();
            StartCoroutine(SpawnPeople());
            StartCoroutine(Questionnaire());
        }
    }

    public IEnumerator SpawnPeople()
    {
        difficulty = Math.Min(2, numCorrect);

        int peopleCount = UnityEngine.Random.Range(6, 10) + UnityEngine.Random.Range(1, 2) * difficulty;
        float peopleWait = 1.25f;

        spawning = true;

        for (int i = 0; i < peopleCount; i++)
        {
            StartCoroutine(PersonControl());

            yield return new WaitForSeconds(UnityEngine.Random.Range(peopleWait, peopleWait + 0.5f));
        }

        spawning = false;
    }

    public IEnumerator PersonControl()
    {
        activePeople++;

        GameObject p;

        // Create
        {
            int selectedSpawner = UnityEngine.Random.Range(0, 3);

            Vector2 spawnerPos = Spawners[selectedSpawner].transform.position;
            spawnerPos.x += UnityEngine.Random.Range(-0.5f, 0.5f);
            p = Instantiate(Person, spawnerPos, Quaternion.identity);
        }

        // Fall
        {
            Vector2 vecA = p.transform.position;
            Vector2 vecB = p.transform.position;
            vecB.y = -1.5f;

            int fallTime = UnityEngine.Random.Range(60, 75);

            for (int i = 1; i <= fallTime; i++)
            {
                p.transform.position = Interpolate_Accelerate(vecA, vecB, i / (fallTime * 1.0f));
                yield return new WaitForEndOfFrame();
            }
        }

        yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.0f));

        // Hide
        {
            int selectedCover = UnityEngine.Random.Range(0, difficulty + 1);

            Vector2 vecA = p.transform.position;
            Vector2 vecB = Covers[selectedCover].transform.position;
            vecB.x += UnityEngine.Random.Range(-0.5f, 0.5f);
            vecB.y = -1.5f;

            int hideTime = UnityEngine.Random.Range(50, 70) * (int)Mathf.Abs(vecA.x - vecB.x);

            for (int i = 1; i <= hideTime; i++)
            {
                p.transform.position = Interpolate_Smooth(vecA, vecB, i / (hideTime * 1.0f));
                yield return new WaitForEndOfFrame();
            }
        }

        activePeople--;
    }

    Vector2 Interpolate_Accelerate(Vector2 v1, Vector2 v2, float t)
    {
        float interpolation = t * t;

        float x = v1.x + (v2.x - v1.x) * interpolation;
        float y = v1.y + (v2.y - v1.y) * interpolation;

        return new Vector2(x, y);
    }

    Vector2 Interpolate_Smooth(Vector2 v1, Vector2 v2, float t)
    {
        float interpolation = t * t * (3 - 2 * t);

        float x = v1.x + (v2.x - v1.x) * interpolation;
        float y = v1.y + (v2.y - v1.y) * interpolation;

        return new Vector2(x, y);
    }

    public IEnumerator Questionnaire()
    {
        questioning = true;

        Result.text = "";

        int ticks = 0;
        while (spawning || 0 < activePeople)
        {
            string dots = "";
            for(int i = 1; i <= ticks % 6; i++)
            {
                dots += ".";
            }

            Query.text = "How many people are behind the" + dots;

            ticks++;
            yield return new WaitForSeconds(1 / 3.0f);
        }

        coverAnswer = UnityEngine.Random.Range(0, difficulty + 1);

        Query.text = "How many people are behind the " + coverTexts[coverAnswer] + "?";

        Result.Select();
        Result.ActivateInputField();

        questioning = false;
    }

    public void ClearPeople()
    {
        GameObject[] people = GameObject.FindGameObjectsWithTag("Person");

        foreach (GameObject p in people)
        {
            GameObject.Destroy(p);
        }
    }

    public void endGame()
    {
        StopAllCoroutines();
        ClearPeople();
        SE.PlayOneShot(timeup);
        StartCoroutine("Timesup");
    }

    IEnumerator Timesup()
    {
        Game.SetActive(false);
        house.SetActive(false);
        rock.SetActive(false);
        tree.SetActive(false);
        ReadyStart.SetActive(true);

        counter.text = "All Done";

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

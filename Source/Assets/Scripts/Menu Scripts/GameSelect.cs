using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelect : MonoBehaviour
{
    public Animator transition;
    private float transitionTime = 2f;

    //Turing Says Button Action
    public void StartTuring()
    {
        StartCoroutine(turTrans());
    }

    IEnumerator turTrans()
    {
        //Start Transition
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(3);
        yield return null;
    }

    //House Game Button Action
    public void StartHouse()
    {
        StartCoroutine(houseTrans());
    }

    IEnumerator houseTrans()
    {
        //Start Transition
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(4);
        yield return null;
    }

    //Basic Arithmetic Button Action
    public void StartMath()
    {
        StartCoroutine(mathTrans());
    }

    IEnumerator mathTrans()
    {
        //Start Transition
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(2);
        yield return null;
    }

    public void mainMenu()
    {
        StartCoroutine("MainMenuTransition");
    }

    IEnumerator MainMenuTransition()
    {
        //Start Transition
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(0);

        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Animator transition;
    private float transitionTime = 2f;

    //Main Menu Objects
    public GameObject MainMenuPanel;
    public GameObject play;
    public GameObject options;
    public GameObject exit;
    public GameObject Steph;
    public GameObject textBubble;

    //Options Menu Objects
    public GameObject OptionsPanel;
    public GameObject MenuButton;
    public TMP_Dropdown dropdownMenu;
    Resolution[] resolutions;

    private void Start()
    {
        List<string>  resOps = new List<string>();
        dropdownMenu.ClearOptions();
        resolutions = new Resolution[8];

        //Add 640 by 480 resolution option
        string option = "640 x 480";
        resOps.Add(option);
        
        Resolution res = new Resolution();
        res.width = 640;
        res.height = 480;
        resolutions[0] = res;
       
        //Add 800 by 600 resolution option
        option = "800 x 600";
        resOps.Add(option);
        
        res.width = 800;
        res.height = 600;
        resolutions[1] = res;
        
        //Add 960 by 720 resolution option
        option = "960 x 720";
        resOps.Add(option);
        
        res.width = 960;
        res.height = 720;
        resolutions[2] = res;
        
        //Add 1280 by 960 resolution option
        option = "1280 x 960";
        resOps.Add(option);
        
        res.width = 1280;
        res.height = 960;
        resolutions[3] = res;
        
        //Add 1440 by 1080 resolution option
        option = "1440 x 1080";
        resOps.Add(option);
        
        res.width = 1440;
        res.height = 1080;
        resolutions[4] = res;
        
        //Add 1600 by 1200 resolution option
        option = "1600 x 1200";
        resOps.Add(option);
        
        res.width = 1600;
        res.height = 1200;
        resolutions[5] = res;
        
        //Add 1920 by 1440 resolution option
        option = "1920 x 1440";
        resOps.Add(option);
        
        res.width = 1920;
        res.height = 1440;
        resolutions[6] = res;
        
        //Add 2880 by 2160 resolution option
        option = "2880 x 2160";
        resOps.Add(option);
        
        res.width = 2880;
        res.height = 2160;
        resolutions[7] = res;
        


        int curRes = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                curRes = i;
            }
        }

        dropdownMenu.AddOptions(resOps);
        dropdownMenu.value = curRes;
        dropdownMenu.RefreshShownValue();

    }

    string ResToString(Resolution res)
    {
        return res.width + " x " + res.height;
    }

    public void setRes(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
    }

    public void playGame()
    {
        StartCoroutine("GameSelectTransition");
    }

    public void optionsMenu()
    {
        StartCoroutine("OptionsTransition");
    }

    //Option Transition
    IEnumerator OptionsTransition()
    {
        //Start Transition
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        //Hide Main Menu
        GameObject.FindGameObjectWithTag("Options").GetComponent<ButtonShake>().setFalse();
        MainMenuPanel.SetActive(false);
        Steph.SetActive(false);
        textBubble.SetActive(false);

        //Show Options Menu
        OptionsPanel.SetActive(true);

        //Finish transition
        transition.SetTrigger("End");
        yield return new WaitForSeconds(1f);

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

        //Hide Options Menu
        OptionsPanel.SetActive(false);

        //Show Main Menu
        MainMenuPanel.SetActive(true);
        Steph.SetActive(true);
        textBubble.SetActive(true);
        GameObject.FindGameObjectWithTag("Steph").GetComponent<StephBounce>().StartCoroutine("Bounce");

        //Finish transition
        transition.SetTrigger("End");
        yield return new WaitForSeconds(1f);

        yield return null;
    }

    IEnumerator GameSelectTransition()
    {
        //Start Transition
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
    }

    public void Exit()
    {
        Application.Quit();
    }

}

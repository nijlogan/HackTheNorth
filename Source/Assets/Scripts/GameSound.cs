using UnityEngine;
using UnityEngine.Audio;

public class GameSound : MonoBehaviour
{
    //Audio Mixer for Volume Controls
    public AudioMixer master;
    private static GameSound instance;

    private void Awake()
    {
        instantiateAudio();
    }

    private void instantiateAudio()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != instance)
        {
            Destroy(this.gameObject);
        }
    }

    //Volume adjustment for Volume Sliders
    public void setMaster(float sliderValue)
    {
        master.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
    }

    public void setMusic(float sliderValue)
    {
        master.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }

    public void setSE(float sliderValue)
    {
        master.SetFloat("SEVol", Mathf.Log10(sliderValue) * 20);
    }

}

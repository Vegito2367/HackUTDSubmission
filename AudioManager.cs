using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject InterNetTing;
    public string ThemeMusic;
    public List<Sound> Sounds = new List<Sound>();
    public static AudioManager instance;

    private void Awake()
    {
        ThemeMusic = PlayerPrefs.GetString("MainMusic", "ThemeSong1");

        Application.targetFrameRate = 900;

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if(!PlayerPrefs.HasKey("FirstTimeDone"))
        {
            PlayerPrefs.SetString("FirstTimeDone", "yes");
        }
        foreach (Sound item in Sounds)
        {
           item.audioSource= gameObject.AddComponent<AudioSource>();
            item.audioSource.clip = item.Clip;
            item.audioSource.pitch = item.pitch;
            item.audioSource.volume = item.Volume;
            item.audioSource.loop = item.loop;
            item.audioSource.outputAudioMixerGroup = item.Output;
            item.audioSource.spatialBlend = item.SpacialBlend;
        }
        Play(ThemeMusic);
    }
    [SerializeField] float CheckRate = 5f;
    float checkTimeForreal;
    
    private void Update()
    {
        //if(Input.GetMouseButtonDown(2))
        //{
        //    PlayerDataStorer.DeleteProgress();
        //}
        if (checkTimeForreal>=CheckRate)
        {
            if(Application.internetReachability==NetworkReachability.NotReachable)
            {
                InterNetTing.SetActive(true);
            }
            checkTimeForreal = 0f;
        }
        else
        {
            checkTimeForreal += Time.deltaTime;
        }
    }

    public void SetMainThemeSong(string s)
    {
        StopMusic(ThemeMusic);
        ThemeMusic = s;
        PlayerPrefs.SetString("MainMusic", s);
        foreach (Sound item in Sounds)
        {
            if (item.SoundName == ThemeMusic)
            {
                item.audioSource.Play();
            }
        }
        
    }
   public void PauseMusic(string name)
   {
        foreach (Sound item in Sounds)
        {
            if (item.SoundName == name)
            {
                item.audioSource.Pause();
            }
        }
   }
    public void StopMusic(string name)
    {
        foreach (Sound item in Sounds)
        {
            if(item.SoundName.Equals(name))
            {
                item.audioSource.Stop();
            }
        }
    }
    public void Play(string name)
    {
        foreach (Sound  item in Sounds)
        {
            if (item.SoundName==name)
            {
                item.audioSource.Play();
            }
        }
    }
    public void UnpauseMusic(string name)
    {
        foreach (Sound item in Sounds)
        {
            if (item.SoundName == name)
            {
                item.audioSource.UnPause();
            }
        }
    }
}

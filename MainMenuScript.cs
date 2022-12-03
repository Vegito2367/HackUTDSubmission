using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
   [SerializeField] GameObject Intro;
   [SerializeField] GameObject Lobby;
   [SerializeField] GameObject PrivacyPolicy;
    bool NotEntered;
    
    private void Start()
    {
        NotEntered = !PlayerPrefs.HasKey("AlreadyEntered");
        //AudioManager.instance.Play(AudioManager.instance.ThemeMusic);
        Intro.SetActive(NotEntered);
        if(!NotEntered)
        {
            Lobby.SetActive(true);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("AlreadyEntered");
    }
    public void EnableLobbyDisableIntro()
    {
        
        Intro.SetActive(false);
        Lobby.SetActive(true);
        PrivacyPolicy.SetActive(true);
        PlayerPrefs.SetInt("AlreadyEntered", 0);
    }
}

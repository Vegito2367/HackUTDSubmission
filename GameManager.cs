using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UnityEvent levelComplete;
    public UnityEvent RestartLevel;
    public UnityEvent StoreData;
    [SerializeField] GameObject LastLevelDialog;
    public static bool GameIsOn = false;
    [SerializeField] GameObject[] Walls;
    public GameObject[] GrapplePoints;
    [SerializeField] private Transform player;
    public Transform fallBarrier;
    public float Range;
    Level_Manager level_Manager;
    public bool IsInTutorial = false;
    public ScoreManager sm;
    public ScoreDisplayer scoreDisplayer;
    public int levelplayed;
    public int Restarts;
    [SerializeField] AdsMidLevel ads;
    #region Level_Functioning
    private void OnEnable()
    {
        levelplayed = 0;
        GameIsOn = true;
        level_Manager = GetComponent<Level_Manager>();
        PlayerDataStorer.LoadProgress();
    }
    public void ResetObstacles()
    {
        for (int i = 0; i < Walls.Length; i++)
        {
            GameObject item = Walls[i];
            if (item != null)
                item = null;
        }
        for (int i = 0; i < GrapplePoints.Length; i++)
        {
            GameObject item = GrapplePoints[i];
            if (item != null)
                item = null;
        }
        Walls = GameObject.FindGameObjectsWithTag("Enviroment");
        GrapplePoints = GameObject.FindGameObjectsWithTag("GrapplePoints");
    }

    public void FinalLevelDialog()
    {
        LastLevelDialog.SetActive(true);
    }

    private void Update()
    {
        ObstaclesEnabling();
        if (player.position.y < fallBarrier.position.y)
        {
            if (!IsInTutorial)
            {
                Restarts += 1;
                ads.RestartLevelAd();
            }
            else
                RestartTheFuckingLevel();
        }
    }
    public void RestartTheFuckingLevel()
    {
        RestartLevel.Invoke();
    }
    private void ObstaclesEnabling()
    {
        for (int i = 0; i < Walls.Length; i++)
        {
            float d = Vector3.Distance(Walls[i].transform.position, player.position);
            if (d > Range)
            {
                Walls[i].SetActive(false);
            }
            else
                Walls[i].SetActive(true);
        }
        for (int i = 0; i < GrapplePoints.Length; i++)
        {
            float d = Vector3.Distance(GrapplePoints[i].transform.position, player.position);
            if (d > Range)
            {
                GrapplePoints[i].SetActive(false);
            }
            else
                GrapplePoints[i].SetActive(true);
        }
    }


    
    public static void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }
    #region PauseMenu
    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void Unpause()
    {
        Time.timeScale = 1;
    }
    public void QuitTheGame()
    {
        Application.Quit();
    }
    #endregion
    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    public static void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public static void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public static void LoadScene(bool LoadNext)
    {
        if (LoadNext)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    #endregion
   
    
    public void CurrentLevelComplete()
    {
        if (IsInTutorial)
        {
            level_Manager.SwitchLevel();
           
            return;
        }
        sm.StopTime();
        levelplayed++;
        GoToJetpack();
        level_Manager.ActivateStoryScreen();           //Calls Endscreeen here
    }
    public void GoToTutorial()
    {
        if (!PlayerDataStorer.HasUnlockedGrappleGun && level_Manager.currentLevel == 5)
        {
            Unpause();
            PlayerDataStorer.HasUnlockedGrappleGun = true;
            LoadScene(4);
            PlayerDataStorer.SaveProgress();
            AudioManager.instance.UnpauseMusic(AudioManager.instance.ThemeMusic);
        }
    }
    public void GoToJetpack()
    {
       
        if (!PlayerDataStorer.hasUnlockedJetpack && level_Manager.currentLevel == 9)
        {
            Unpause();
            PlayerDataStorer.hasUnlockedJetpack = true;
            LoadScene(5);
            PlayerDataStorer.SaveProgress();
            AudioManager.instance.UnpauseMusic(AudioManager.instance.ThemeMusic);
        }
    }
    public void SWITCHLEVEL()
    {
        levelComplete.Invoke();
    }
    public void SaveData()
    {
        if (!IsInTutorial)
        {
            PlayerPrefs.SetInt("IsUnlocked" + level_Manager.currentLevel, 1);
        }
        PlayerDataStorer.SaveProgress();
    }

    public void EndScreen()
    {
        scoreDisplayer.StartEndScreen();
    }
    public void ZTakeToCredits()
    {
        LoadScene(6);
    }
}

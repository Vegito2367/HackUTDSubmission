using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ChangeScene : MonoBehaviour
{
   [SerializeField] Button[] Buttons;
    public GameObject TutorialDialog;
    [SerializeField] GameObject Loading;
    int hasCompletedTutorial;
   [SerializeField] private GameObject StoryThing;

    private void Start()
    {
        PlayerDataStorer.LoadProgress();
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = PlayerPrefs.GetInt("IsUnlocked" + (i+1), 0) == 1;
        }
    }
    public void SceneChange(int id)
    {
        Loading.SetActive(true);
        AsyncOperation asy= SceneManager.LoadSceneAsync("Level_1");
        PlayerPrefs.SetInt("LevelToBeExecuted", id - 1);
        DoLoadingScreen(asy,Loading);
    }
    public void NextScene()
    {
        hasCompletedTutorial = PlayerPrefs.GetInt("TutorialComplete", 0);
        if (hasCompletedTutorial==0)
        {
            SetTutorialQuestionActive();

        }
        else
        {
           if(PlayerDataStorer.FirstStoryDone)
           {
                LoadThePlayLevel();
                
           }
           else
           {
                StoryThing.SetActive(true);
           }
        }
    }

    public void LoadThePlayLevel()
    {
        AsyncOperation asy = SceneManager.LoadSceneAsync(1);
        Loading.SetActive(true);
        DoLoadingScreen(asy, Loading);
    }

    public static void DoLoadingScreen(AsyncOperation operation_,GameObject Parent)
    {
        Slider LoadingSlider = Parent.transform.GetChild(1).gameObject.GetComponent<Slider>();
        LoadingSlider.value = operation_.progress+0.1f;
    }
    private void SetTutorialQuestionActive()
    {
        TutorialDialog.SetActive(true);
    }
    public void BackTomainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

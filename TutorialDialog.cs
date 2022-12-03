using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TutorialDialog : MonoBehaviour
{
   [SerializeField] GameObject StoryCrap;
    [SerializeField] GameObject Loading;
   public void TutorialChoose(bool GoToTutorial)
   {
        Loading.SetActive(true);
        PlayerPrefs.SetInt("TutorialComplete", 1);
        if (GoToTutorial)
        {
           AsyncOperation asy= SceneManager.LoadSceneAsync("1_tutorial");
            ChangeScene.DoLoadingScreen(asy,Loading);
        }
        else
        {
            StoryCrap.SetActive(true);
        }
        
   }
}

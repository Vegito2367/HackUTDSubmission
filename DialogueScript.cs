using System.Collections;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScript : MonoBehaviour
{
    public ConversationType conversationType;
    public TextMeshProUGUI TargetText;
    public float WaitTime = 0.1f;
    int currentint;
    public Color VirusColor;
    public Color MyColor;
    public UnityEvent AfterDialogue;
    [TextArea(10,100)]
    public List<string> TestTexts = new List<string>();
    

    IEnumerator DisplayTextProcedural(string s,Color color)
    {
        TargetText.text = "";
        TargetText.color = color;
        for (int i = 0; i < s.Length; i++)
        {
            if(s[i]=='-')
            {
                TargetText.text += "\n";
                continue;
            }
            if(s[i]=='/')
            {
                yield return new WaitForSeconds(2f);
                continue;
            }
            TargetText.text += s[i];
            yield return new WaitForSeconds(WaitTime);
        }
    }
    public void AfterDialogueMethod()
    {
        AfterDialogue.Invoke();
    }
    private void OnEnable()
    {
        NextText();
        EnableStoryMusic();
    }
    public void EnableStoryMusic()
    {
        AudioManager.instance.PauseMusic(AudioManager.instance.ThemeMusic);
        AudioManager.instance.Play("StoryMusic");
        
    }
    public void DisableStoryMusic()
    {
        AudioManager.instance.StopMusic("StoryMusic");
        AudioManager.instance.UnpauseMusic(AudioManager.instance.ThemeMusic);

    }
    public void NextText()
    {
        StopAllCoroutines();
        if (currentint >= TestTexts.Count)
        {

            TargetText.text = "";
            SaveStory();
            AfterDialogueMethod();
            return;
        }
        bool ColorDecider = (conversationType == ConversationType.Dialogue) ? currentint % 2 == 0 : true;
        Color c = (ColorDecider) ? VirusColor : MyColor;
        StartCoroutine(DisplayTextProcedural(TestTexts[currentint],c));
        currentint++;
    }

    public void SaveStory()
    {
        PlayerDataStorer.FirstStoryDone = true;
        PlayerDataStorer.SaveProgress();
    }

}

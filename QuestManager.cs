using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> Quests = new List<Quest>();
    public Text QuestText;
    public Quest ActiveQuest;
    public int currentQuestIndex;
    [SerializeField] Transform player;
    [SerializeField] Level_Manager lm;
    void Start()
    {
        
        for (int i = 0; i < Quests.Count; i++)
        {
            if(Quests[i].IsActive)
            {
                ActivateCurrentQuest(Quests[i]);
                lm.LoadLevelWIthIndex(i);
                lm.currentLevel = i;
                currentQuestIndex = i;
                break;
            }
        }
    }
    void ActivateCurrentQuest(Quest q)
    {
        ActiveQuest = null;
        ActiveQuest = q;
        ActiveQuest.IsActive = true;
        ActiveQuest.isComplete = false;
        ChangeText();
        
    }
    public void CurrentQuestCompleted()
    {
        currentQuestIndex++;
        ActiveQuest.CompleteEvent.Invoke();
        ActiveQuest.IsActive = false;
        ActivateCurrentQuest(Quests[currentQuestIndex]);
    }
    public void ChangeText()
    {
        QuestText.text = ActiveQuest.QuestText;
    }
    public void ChangeText(string text)
    {
        QuestText.text = text;
    }
}

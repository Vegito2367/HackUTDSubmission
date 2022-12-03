using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class Quest
{
    [TextArea]
    public string QuestText;
    public UnityEvent CompleteEvent;
    public bool IsActive;
    public bool isComplete;
}

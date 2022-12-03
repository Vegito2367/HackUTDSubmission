using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public Text ScoreText;
    float TimeTaken = 0f;
    public bool ContinueTimer = true;
    public int CompleteCoins;
    private void Start()
    {
        CompleteCoins = PlayerDataStorer.totalcoins;
    }
    void Update()
    {
        ScoreText.text = TimeTaken.ToString("0.00");
        if(ContinueTimer)
        TimeTaken += Time.deltaTime;
    }

    public void RestartTimer()
    {
        TimeTaken = 0f;
        ContinueTimer = true;
    }
    public void SaveCoins()
    {
        PlayerDataStorer.totalcoins = CompleteCoins;
    }
    public void StopTime()
    {
        ContinueTimer = false;
    }
    
    public float GetTime()
    {
        return TimeTaken;
    }
}

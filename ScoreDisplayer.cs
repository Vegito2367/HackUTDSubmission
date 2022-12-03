using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using TMPro;
using UnityEngine;

public class ScoreDisplayer : MonoBehaviour
{
    [SerializeField] Level_Manager levelManager;
    [SerializeField] GameObject EndScreen;
    [SerializeField] TextMeshProUGUI TimeText;
    [SerializeField] TextMeshProUGUI Multiplier;
    [SerializeField] TextMeshProUGUI TotalCoins;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] private float UpdateTime;
    public UnityEvent AdThing;
    public void StartEndScreen()
    {
        StartCoroutine(ActivateEndscreen());
    }
    public void UpdateAfterAd()
    {
        int newCoins = Mathf.FloorToInt(1.5f * TotalCoinsValue);
        StartCoroutine(UpdateValueButLikeExcess(newCoins, TotalCoinsValue, TotalCoins));
    }
    int TotalCoinsValue;
    private IEnumerator ActivateEndscreen()
    {
        float t = scoreManager.GetTime();
        TotalCoinsValue = levelManager.CalculateLevelCoins(t);
        if (TotalCoinsValue > 100)
        {
            UpdateTime = 0.005f;
        }
        else
        {
            UpdateTime = 0.01f;
        }
        EndScreen.SetActive(true);
        TimeText.text = "0";
        Multiplier.text = "0";
        TotalCoins.text = "0";
        scoreManager.CompleteCoins += TotalCoinsValue;
        scoreManager.SaveCoins();
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(UpdateValueInAFunWay(t, TimeText));
        yield return new WaitForSeconds(0.5f);
        float multiplier = levelManager.GetMultiplier();
        
        
        StartCoroutine(UpdateValueInAFunWay(multiplier, Multiplier));
        yield return new WaitForSeconds(0.5f);
        
        
        StartCoroutine(UpdateValueInAFunWay(TotalCoinsValue,TotalCoins));

    }

    IEnumerator UpdateValueInAFunWay(int Val, TextMeshProUGUI textbox)
    {
        int k = 0;
        while(k<Val)
        {
            k++;
            textbox.text = k.ToString("0");
            yield return new WaitForSeconds(UpdateTime);
        }
        if(Val==TotalCoinsValue)
        {
            AdThing.Invoke();
        }
    }
    IEnumerator UpdateValueButLikeExcess(int Val,int original, TextMeshProUGUI textbox)
    {
        int k = original;
        while (k < Val)
        {
            k++;
            textbox.text = k.ToString("0");
            yield return new WaitForSeconds(UpdateTime);
        }
        scoreManager.CompleteCoins -= TotalCoinsValue;
        scoreManager.CompleteCoins += Val;
        scoreManager.SaveCoins();
    }
    IEnumerator UpdateValueInAFunWay(float Val, TextMeshProUGUI textbox)
    {
        
        float k = 0;
        while (k < Val)
        {
            k++;
            textbox.text = k.ToString("0");
            yield return new WaitForSeconds(UpdateTime);
            
        }
    }
    public int GetCoins() => TotalCoinsValue;
    public void SaveThefuckingData()
    {
        levelManager.GetComponent<GameManager>().StoreData.Invoke();
    }
}

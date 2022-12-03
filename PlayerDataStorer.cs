using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class PlayerDataStorer
{
    //public static int levelsComplete;
    public static int totalcoins;
    public static int JetpackUpgradeLevel;
    public static int GrappleUpgradeLevel;
    public static bool hasUnlockedJetpack=false;
    public static bool HasUnlockedGrappleGun=false;
    public static bool FirstStoryDone;
    public static void SaveProgress()
    {
        SaveToBinary.SavePlayerData();
        LoadProgress();
    }
    public static void LoadProgress()
    {
        PlayerData pd= SaveToBinary.LoadPlayerData();
        totalcoins = pd.TotalCoins;
        JetpackUpgradeLevel = pd.JetPackLevel;
        GrappleUpgradeLevel = pd.JetPackLevel;
        hasUnlockedJetpack = pd.UnlockedJetpack;
        HasUnlockedGrappleGun = pd.UnlockedGrapple;
        FirstStoryDone = pd.FirstStoryDone;
        
    }
    public static void DeleteProgress()
    {
        totalcoins = 0;
        JetpackUpgradeLevel = 0;
        GrappleUpgradeLevel = 0;
        hasUnlockedJetpack = false;
        HasUnlockedGrappleGun = false;
        FirstStoryDone = false;
        SaveProgress();
        LoadProgress();
        PlayerPrefs.DeleteAll();
    }
}

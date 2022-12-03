using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData 
{
    //public int LevelsCompleted;
    public int TotalCoins;
    public int JetPackLevel;
    public int GrappleLevel;
    public bool UnlockedJetpack;
    public bool UnlockedGrapple;
    public bool FirstStoryDone;
    
    public PlayerData()
    {
        TotalCoins += PlayerDataStorer.totalcoins;
        JetPackLevel = PlayerDataStorer.JetpackUpgradeLevel;
        GrappleLevel = PlayerDataStorer.GrappleUpgradeLevel;
        UnlockedGrapple = PlayerDataStorer.HasUnlockedGrappleGun;
        UnlockedJetpack = PlayerDataStorer.hasUnlockedJetpack;
        FirstStoryDone = PlayerDataStorer.FirstStoryDone;
        
    }
}

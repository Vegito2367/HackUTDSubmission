using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level_Structure
{
    public int Level_Index;
    public GameObject Level_Enviroment;
    public bool AllowGrappling;
    public bool AllowJetpack;
    public float FiveXMultiplierTime;
    public float TenXMultiplierTime;
    public GameObject EndStoryScreen;
}

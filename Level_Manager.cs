using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Level_Manager : MonoBehaviour
{
    
    GameManager gm;
    public Transform player;
    public int currentLevel;
    GameObject currentEnviroment;
    Transform SpawnPos;
    public GameObject GrappleButton;
    public GameObject GrapplingGun;
    public GameObject JetPackButton;
    public JetpackScript script;
    public GameObject slider;
    public List<Level_Structure> Level_Structures = new List<Level_Structure>();
    
    private void Awake()
    {
        gm = GetComponent<GameManager>();
        
            if (!gm.IsInTutorial)
                currentLevel = PlayerPrefs.GetInt("LevelToBeExecuted", 0);
       
    }
   
    private void Start()
    {
        
        for (int i = 0; i < Level_Structures.Count; i++)
        {
            if(Level_Structures[i].Level_Index==currentLevel)
            {
                currentEnviroment= Level_Structures[i].Level_Enviroment;
                currentEnviroment.SetActive(true);
                Transform t = currentEnviroment.transform.GetChild(1);
                player.position = t.position;
                gm.fallBarrier= currentEnviroment.transform.GetChild(0);
                player.rotation = t.localRotation;
                SpawnPos = t;
                GrappleButton.SetActive(Level_Structures[currentLevel].AllowGrappling);
                GrapplingGun.SetActive(GrappleButton.activeSelf);
                JetPackButton.SetActive(Level_Structures[currentLevel].AllowJetpack);
                script.enabled = JetPackButton.activeSelf;
                slider.SetActive(JetPackButton.activeSelf);
            }
        }
        gm.ResetObstacles();
    }
    [Header("Only For Tutorial")]
    [SerializeField] GameObject EndTut;
    public void SwitchLevel()
    {
        currentEnviroment.SetActive(false);
        currentLevel++;
        if(gm.IsInTutorial)
        {
            if(currentLevel>=Level_Structures.Count)
            {
                EndTut.SetActive(true);
                return;
            }
        }
        currentEnviroment = Level_Structures[currentLevel].Level_Enviroment;
        currentEnviroment.SetActive(true);
        SpawnPos = currentEnviroment.transform.GetChild(1);
        player.position = SpawnPos.position;
        gm.ResetObstacles();
        gm.fallBarrier = currentEnviroment.transform.GetChild(0);
        player.rotation = SpawnPos.localRotation;
        SetPowerupsActive();
        PlayerDataStorer.HasUnlockedGrappleGun = PlayerPrefs.GetInt("IsUnlocked6", 0) == 1;
        PlayerDataStorer.hasUnlockedJetpack = PlayerPrefs.GetInt("IsUnlocked15") == 1;
        script.currentTime = 0;
    }

    private void SetPowerupsActive()
    {
        GrappleButton.SetActive(Level_Structures[currentLevel].AllowGrappling);
        GrapplingGun.SetActive(GrappleButton.activeSelf);
        JetPackButton.SetActive(Level_Structures[currentLevel].AllowJetpack);
        script.enabled = JetPackButton.activeSelf;
        slider.SetActive(JetPackButton.activeSelf);
    }

    public void ActivateStoryScreen()
    { 
        Level_Structures[currentLevel].EndStoryScreen.SetActive(true);
    }
    public void LoadLevelWIthIndex(int Index)
    {
        if(currentEnviroment!=null)
        currentEnviroment.SetActive(false);
        currentLevel=Index;
        currentEnviroment = Level_Structures[currentLevel].Level_Enviroment;
        currentEnviroment.SetActive(true);
        SpawnPos = currentEnviroment.transform.GetChild(1);
        player.position = SpawnPos.position;
        gm.ResetObstacles();
        gm.fallBarrier = currentEnviroment.transform.GetChild(0);
        player.rotation = SpawnPos.localRotation;
        script.currentTime = 0;
    }
    public void RestartLevel()
    {
        player.position = SpawnPos.position + Vector3.up * 2f;
        player.rotation = SpawnPos.localRotation;
        Rigidbody rigidbody = player.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        if (!script.isCoolingDown())
            script.currentTime = 0;
    }
    int multiplier;
    public  int CalculateLevelCoins(float time)
    {
        if (gm.IsInTutorial)
            return 0;
        int Coins;
        float multiplyTime=0;
        if(time>Level_Structures[currentLevel].FiveXMultiplierTime)
        {
            multiplier = 1;
            multiplyTime = 20;
        }
        else if (time > Level_Structures[currentLevel].TenXMultiplierTime && time < Level_Structures[currentLevel].FiveXMultiplierTime)
        {
            multiplier = 10;
            multiplyTime = Level_Structures[currentLevel].FiveXMultiplierTime - time;
        }
        else if (time<Level_Structures[currentLevel].TenXMultiplierTime)
        {
            multiplier = 20;
            if (time > 50f)
            {
                multiplyTime = time - 50f;
            }
            else
                multiplyTime = time;
        }
        Coins = Mathf.FloorToInt(multiplyTime * multiplier);

        return Coins;
    }
    public int GetMultiplier()
    {
        return multiplier;
    }
}

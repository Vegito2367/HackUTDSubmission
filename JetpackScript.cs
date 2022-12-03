using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System;

public class JetpackScript : MonoBehaviour
{
    
    [SerializeField] float cooldownTime;
    [SerializeField] float JetPackTime;

    public float UpForce;
    public bool isJetpacking = false;
    bool isCoolDown = false;
    public float currentTime;
    public Slider slider;
    Image sliderImage;


    [SerializeField] Rigidbody rb;
    [SerializeField] move MoveScript;
    [SerializeField] WallRunBackup wb;
    [SerializeField] private float RateOfRefilling = 1f;
    [SerializeField] private float RateofUse;
    [SerializeField] MyButton JetPackButton;
    [SerializeField] Button JumpButton;
    [SerializeField] ParticleSystem WindParticle;
    bool ButtonisPressed;
    int JetPackUpgradeLevel;
    bool StartAlwaysCoolDown = false;

    void Start()
    {
        sliderImage = slider.fillRect.GetComponent<Image>();
        JetPackUpgradeLevel = PlayerDataStorer.JetpackUpgradeLevel;
        if (!SceneManager.GetActiveScene().name.Equals("Level_1"))
            ApplyUgrade(0);
        else
            ApplyUgrade(JetPackUpgradeLevel);
    }

    private void ApplyUgrade(int JetPackUpgradeLevel)
    {
        cooldownTime = 10f - (JetPackUpgradeLevel * .7f);
        RateOfRefilling = 0.1f + (JetPackUpgradeLevel * 0.15f);
        RateofUse = 2f - (JetPackUpgradeLevel * .15f);
    }

    public void InputJetPack()
    {

        if (ButtonisPressed && currentTime >= 0 && !isCoolDown&&!wb.isWallRunning)
        {
            ExecuteJetPack();
        }
        else
        {
            isJetpacking = false;
            if (WindParticle.isPlaying)
            {
                StopCoroutine(CoolDelay());
                StartCoroutine(CoolDelay());
                WindParticle.Stop();
            }
        }
    }
    IEnumerator CoolDelay()
    {
        yield return new WaitForSeconds(.5f);
        StartAlwaysCoolDown = true;
    }
    void ExecuteJetPack()
    {

        if (currentTime <= JetPackTime)
        {
            isJetpacking = true;
            rb.AddForce(0, UpForce * Time.deltaTime, 0, ForceMode.VelocityChange);
            currentTime += RateofUse*Time.deltaTime;
            WindParticle.gameObject.SetActive(true);
            if (!WindParticle.isPlaying)
            {
                StartAlwaysCoolDown = false;
                WindParticle.Play();
            }
        }
        else
        {
            isJetpacking = false;
            if (WindParticle.isPlaying)
            {
                WindParticle.Stop();
            }
            return;
        }
    }
    
    void Update()
    {
        CheckIfInAir(); 
        InputJetPack();

        ButtonisPressed = JetPackButton.IsPressed();
        
        if (currentTime > JetPackTime && !isCoolDown)
        {
            StartCoroutine(DoCoolDown());
        }
        if (isCoolDown)
        {
            currentTime -= (JetPackTime / cooldownTime) * Time.deltaTime;
            if (currentTime < 0)
                currentTime = 0;
        }
        slider.value = (JetPackTime - currentTime) / JetPackTime;

        if(StartAlwaysCoolDown)
        AlwaysCoolDown();

        if (isCoolDown)
            return;
        if (slider.value == slider.maxValue)
        {
            sliderImage.color = Color.red;
        }
        else
        {
            sliderImage.color = Color.blue;
        }



    }

    private void CheckIfInAir()
    {
        if (!MoveScript.IsGrounded()&& !wb.isWallRunning)
        {
            JumpButton.gameObject.SetActive(false);
            JetPackButton.gameObject.SetActive(true);
        }
        else
        {
            JumpButton.gameObject.SetActive(true);
            JetPackButton.gameObject.SetActive(false);
        }
    }

    void AlwaysCoolDown()
    {
        if (currentTime > 0 && !isCoolDown && !isJetpacking)
        {
            currentTime -= RateOfRefilling * Time.deltaTime;
            if (currentTime < 0)
                currentTime = 0;
        }
    }
    private IEnumerator DoCoolDown()
    {
        isCoolDown = true;
        sliderImage.color = Color.yellow;
        yield return new WaitForSeconds(cooldownTime);
        isCoolDown = false;
    }
    public bool isCoolingDown()
    {
        return isCoolDown;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider Volume;
    [SerializeField] Slider Sensitivity;
    [SerializeField] Slider JoystickSize;
    [SerializeField] RectTransform Joystick;
    private void OnEnable()
    {
        Volume.value = PlayerPrefs.GetFloat("VolValue", 0f);
        Sensitivity.value = PlayerPrefs.GetFloat("SensValue", 200f);
        JoystickSize.value = PlayerPrefs.GetFloat("SizeValue", 0.7f);
    }
    public void SetSensitivity(float f)
    {
        PlayerPrefs.SetFloat("Sensitivity", f);
        PlayerPrefs.SetFloat("SensValue", Sensitivity.value);
    }
    public void SetVolume(float f)
    {
        audioMixer.SetFloat("Volume", f);
        PlayerPrefs.SetFloat("VolValue", Volume.value);
    }
    public void SetSize(float f)
    {
        Joystick.localScale = new Vector3(f, f, f);
        PlayerPrefs.SetFloat("JoystickSize", f);
        PlayerPrefs.SetFloat("SizeValue", JoystickSize.value);
    }
}

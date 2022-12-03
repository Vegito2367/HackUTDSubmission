using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwayScript : MonoBehaviour
{
    public float SwaySpeed;
    public float SwayBeginningDelay = 0f;
    public float TimePeriod;
    public bool IsLeftWall;
    float forward;
    void Start()
    {

        if (IsLeftWall) //Setting direction of wall
        {
            forward = -1f;
        }
        else
        {
            forward = 1f;
        }
        InvokeRepeating(nameof(StartSway), SwayBeginningDelay,TimePeriod/2);
    }
    void StartSway()
    {
        forward = -forward;
    }
    
    void Update()
    {
        transform.Translate(SwaySpeed * forward * Time.deltaTime, 0f, 0f);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderTimerUI : MonoBehaviour
{
    [SerializeField] private Image timerImg;
    private float timer;
    private float maxTimer;
    public event EventHandler OnOrderTimeout;

    private void Start()
    {
        timerImg.fillAmount = 1;
    }
    public void SetTimer(float t)
    {
        timer = t;
        maxTimer = t;
    }

    private void Update()
    {
        if(timer>0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Se acabo el tiempo, receta afuera ");
        }
        
    }
}

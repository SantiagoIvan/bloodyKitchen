using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderTimerUI : MonoBehaviour
{
    [SerializeField] private Image timerImg;
    [SerializeField] private OrderTimer orderTimer;
    private void Awake()
    {
        //gameObject.SetActive(false);
    }
    private void Start()
    {
        timerImg.fillAmount = 1;
    }
    private void Update()
    {
        timerImg.fillAmount = orderTimer.GetNormalizedTimer();
    }
}

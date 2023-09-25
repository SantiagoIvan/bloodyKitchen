using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSound : MonoBehaviour
{
    [SerializeField] private OrderTimer orderTimer;
    [SerializeField] private AudioSource audio;
    private void Start()
    {
        orderTimer.OnCloseToTimeout += OrderTimer_OnCloseToTimeout;
    }

    private void OrderTimer_OnCloseToTimeout(object sender, System.EventArgs e)
    {
        audio.Play();
    }
}

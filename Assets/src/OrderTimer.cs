using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderTimer : MonoBehaviour
{
    private int orderId;
    private float timer;
    private float maxTimer;
    private bool closeTimeoutTriggered = false;

    public static event EventHandler<OnOrderTimeOutEventArgs> OnOrderTimeout;
    public event EventHandler OnCloseToTimeout;

    public class OnOrderTimeOutEventArgs
    {
        public int id;
    }
    public int GetOrderId() { return orderId; }
    public void SetOrderId(int id)
    {
        orderId = id;
    }
    public void SetTimer(float t)
    {
        timer = t;
        maxTimer = t;
    }
    public float GetNormalizedTimer()
    {
        return timer / maxTimer;
    }
    private void Update()
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer < maxTimer / 3 && !closeTimeoutTriggered)
            {
                OnCloseToTimeout?.Invoke(this, EventArgs.Empty);
                closeTimeoutTriggered = true;
            }
        }
        else
        {
            OnOrderTimeout?.Invoke(this, new OnOrderTimeOutEventArgs { id = orderId});
        }
    }
}

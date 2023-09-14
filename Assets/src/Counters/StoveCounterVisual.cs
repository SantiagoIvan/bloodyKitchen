using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StoveCounter;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject stoveOnVisualObject;
    [SerializeField] private GameObject particleSystemObject;
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, OnStateChangedEventArgs e)
    {
        switch (e.state)
        {
            case State.IDLE:
                stoveOnVisualObject.SetActive(false);
                particleSystemObject.SetActive(false);
                break;
            case State.FRYING:
                stoveOnVisualObject.SetActive(true);
                particleSystemObject.SetActive(true);
                break;
            case State.FRIED:
                break;
            case State.BURNED: 
                // TODO aca agregar la llamita
                break;
        }
    }
}

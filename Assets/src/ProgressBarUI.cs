using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CuttingCounter;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image progressBarImg;
    [SerializeField] private CuttingCounter cuttingCounter;
    private void Start()
    {
        cuttingCounter.OnProgressChanged += CuttingCounter_OnProgressChanged;
        progressBarImg.fillAmount = 0;
        Hide(); // Si hago el hide ANTES del listener, nunca se va a crear, por eso se hace aca, y no se hace antes ni en el awake que se ejecuta antes 
    }

    private void CuttingCounter_OnProgressChanged(object sender, OnProgressChangedEventArgs e)
    {
        progressBarImg.fillAmount = e.currentProgress;
        if(progressBarImg.fillAmount == 0f || progressBarImg.fillAmount == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

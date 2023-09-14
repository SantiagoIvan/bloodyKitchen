using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private Image progressImg;
    [SerializeField] private GameObject progressContainer;

    private IObjectWithProgress objectWithProgress;
    private void Start()
    {
        objectWithProgress = progressContainer.GetComponent<IObjectWithProgress>();
        if (objectWithProgress == null) {
            Debug.LogError("El progreso no tiene un container correcto");
        }
        progressImg.fillAmount = 0;
        objectWithProgress.OnProgressChanged += ObjectWithProgress_OnProgressChanged;
        Hide(); // Si hago el hide ANTES del listener, nunca se va a crear, por eso se hace aca, y no se hace antes ni en el awake que se ejecuta antes 
    }

    private void ObjectWithProgress_OnProgressChanged(object sender, IObjectWithProgress.OnProgressChangedEventArgs e)
    {
        progressImg.fillAmount = e.currentProgress;
        if(progressImg.fillAmount == 0f || progressImg.fillAmount == 1f)
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

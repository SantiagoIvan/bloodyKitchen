using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameManager;

public class GameplayTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private GameManager gameManager;
    private float timer = 0;

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnGameStarted += Instance_OnGameStarted;
    }

    private void Instance_OnGameStarted(object sender, OnGameStartedEventArgs e)
    {
        timer = e.limitTime;
    }

    private void Update()
    {
        if (gameManager.IsGamePlaying())
        {
            timer -= Time.deltaTime;
            UpdateVisual();
        }
    }
    private void UpdateVisual()
    {
        timerText.text = Math.Ceiling(timer).ToString();
    }
}

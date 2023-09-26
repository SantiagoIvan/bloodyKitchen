using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class GameplayTimerUI : MonoBehaviour
{
    [SerializeField] private Image timerImg;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private Animator animator;
    private const float threshold = 0.2f;
    private const string CLOSE_TO_END_GAME = "CloseToEnd";

    private void Start()
    {
        timerImg.fillAmount = 1;
    }
    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            float timer = gameTimer.GetNormalizedTime();
            Debug.Log("timer: "+timer.ToString());
            timerImg.fillAmount = timer;

            if(timer < threshold) {
                animator.SetBool(CLOSE_TO_END_GAME, true);
            }
        }
    }
}

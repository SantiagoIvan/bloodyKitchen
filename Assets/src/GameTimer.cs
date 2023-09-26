using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private GameManager gameManager;
    private float timer;
    private float maxTime;

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnGameStarted += GameManager_OnGameStarted; ;
    }

    private void GameManager_OnGameStarted(object sender, GameManager.OnGameStartedEventArgs e)
    {
        timer = e.limitTime;
        maxTime = e.limitTime;
    }
    private void Update()
    {
        if (gameManager.IsGamePlaying())
        {
            timer -= Time.deltaTime;
        }
    }
    public float GetNormalizedTime()
    {
        return timer / maxTime;
    }
}

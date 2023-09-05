using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event EventHandler OnStateChanged;
    public event EventHandler<OnCountdownEventArgs> OnCountdown;
    public event EventHandler OnGameOver;
    public event EventHandler<OnGameStartedEventArgs> OnGameStarted;

    public class OnGameStartedEventArgs
    {
        public float limitTime;
    }
    public class OnCountdownEventArgs
    {
        public int count;
    }
    public static GameManager Instance {  get; private set; }
    private enum State
    {
        WAITING_TO_START,
        COUNTDOWN_TO_START,
        PLAYING,
        GAME_PAUSED,
        GAME_OVER
    }

    private State state;

    private const float GAMEPLAY_TIME_LIMIT = 20;
    private float waitingToStartTimer = 1f; // este es simplemente para que todo arranque
    private float countdownToStartTimer = 3f; // 3,2,1,GOO
    private float gamePlayingTimer = GAMEPLAY_TIME_LIMIT; // 1 min de gameplay

    // va a tener mas sentido en el multiplayer cuando estemos esperando a que todos los jugadores tengan este estado para empezar
    private void Awake()
    {
        Instance = this;
        state = State.WAITING_TO_START;
    }


    private void Update()
    {
        switch (state)
        {
            case State.WAITING_TO_START:
                waitingToStartTimer -= Time.deltaTime;
                if(waitingToStartTimer < 0)
                {
                    state = State.COUNTDOWN_TO_START;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.COUNTDOWN_TO_START:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0)
                {
                    state = State.PLAYING;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                    OnGameStarted?.Invoke(this, new OnGameStartedEventArgs { limitTime = GAMEPLAY_TIME_LIMIT});
                }
                break;
            case State.PLAYING:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0)
                {
                    state = State.GAME_OVER;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                    OnGameOver?.Invoke(this, EventArgs.Empty);
                }
                break;
        }
    }

    // hago que los botones funcionen siempre y cuando esté jugando (Player Controller)
    public bool IsGamePlaying()
    {
        return state == State.PLAYING;
    }

    public bool IsCountdownActive() { return state == State.COUNTDOWN_TO_START; }
    public float GetCountdownTimer()
    {
        return countdownToStartTimer;
    }
    public bool IsGameOver() { return state == State.GAME_OVER;}
}

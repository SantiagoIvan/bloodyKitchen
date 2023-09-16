using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public event EventHandler OnStateChanged;
    public event EventHandler<OnCountdownEventArgs> OnCountdown;
    public event EventHandler OnGameOver;
    public event EventHandler<OnGameStartedEventArgs> OnGameStarted;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    [SerializeField] private GameInput gameInput;
    

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
    private State prevState;

    private const float GAMEPLAY_TIME_LIMIT = 60;
    private float countdownToStartTimer = 3f; // 3,2,1,GOO
    private float gamePlayingTimer = GAMEPLAY_TIME_LIMIT; // 1 min de gameplay

    // va a tener mas sentido en el multiplayer cuando estemos esperando a que todos los jugadores tengan este estado para empezar
    private void Awake()
    {
        Instance = this;
        state = State.WAITING_TO_START;
    }

    private void Start()
    {
        gameInput.OnPauseAction += GameInput_OnPauseAction;
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(state == State.WAITING_TO_START)
        {
            state = State.COUNTDOWN_TO_START;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        if (IsGamePlaying() || IsGamePaused())
        {
            TogglePause();
        }
    }

    private void Update()
    {
        switch (state)
        {
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
            case State.GAME_PAUSED:
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

    public void TogglePause()
    {
        if (!IsGamePaused())
        {
            prevState = state;
            Time.timeScale = 0f; // se detiene el tiempo
            state = State.GAME_PAUSED;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            state = prevState;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsGamePaused() { return state == State.GAME_PAUSED;}
    public float GetCountdownToStartTimer() { return countdownToStartTimer; }
}

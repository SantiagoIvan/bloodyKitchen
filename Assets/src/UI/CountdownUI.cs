using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    private GameManager gameManager;
    [SerializeField] private SoundManager soundManager;
    private Animator animator;
    private const string NUMBER_POP_UP_TRIGGER = "NumberPopUp";
    private int prevNumber;
    /* se debe comunicar con el game manager por medio de eventos para saber cuando el juego esta en estado 
     * de juego está, y si está en el countdown, bueno, para que sepa por qué numerito va. 
     */

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnStateChanged += GameManager_OnStateChanged;
        prevNumber = Mathf.CeilToInt(gameManager.GetCountdownToStartTimer());
        Hide();
    }


    private void Update()
    {
        if(gameManager.IsCountdownActive())
        {
            int currentCounter = Mathf.CeilToInt(gameManager.GetCountdownTimer());
            countdownText.text = currentCounter.ToString();
            Debug.Log(currentCounter);
            if(currentCounter != prevNumber) {
                prevNumber = currentCounter;
                animator.SetTrigger(NUMBER_POP_UP_TRIGGER);
                soundManager.PlayCountdownSound();
            }
        }
    }
    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (gameManager.IsCountdownActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        Debug.Log("Showing");
        gameObject.SetActive(true);
        soundManager.PlayCountdownSound(); // para que el primer "tic" se sincronice con la aparición del primer número
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

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
    /* se debe comunicar con el game manager por medio de eventos para saber cuando el juego esta en estado 
     * de juego está, y si está en el countdown, bueno, para que sepa por qué numerito va. 
     */

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }


    private void Update()
    {
        double timer = Math.Ceiling(gameManager.GetCountdownTimer());
        countdownText.text = timer.ToString();
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
        countdownText.gameObject.SetActive(true);
    }
    private void Hide()
    {
        countdownText.gameObject.SetActive(false);
    }
}

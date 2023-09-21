using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private SelectDifUI selectDifUI;

    private void Awake()
    {
        playBtn.onClick.AddListener(OnPlayClick);
        quitBtn.onClick.AddListener(OnQuitClick);
        Time.timeScale = 1.0f; // si voy al menu cuando tiro pausa, se me queda clavado el timescale=0 de cuando pongo pausa.
    }

    private void OnPlayClick()
    {
        selectDifUI.Show();
        Hide();
    }
    private void OnQuitClick()
    {
        Application.Quit();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
}

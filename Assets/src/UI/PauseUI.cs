using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button quitBtn;

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnGamePaused += GameManager_OnGamePaused;
        gameManager.OnGameUnpaused += GameManager_OnGameUnpaused;
        resumeBtn.onClick.AddListener(() => { GameManager.Instance.TogglePause(); });
        quitBtn.onClick.AddListener(() => { SceneLoader.Load(SceneLoader.Scene.MainMenuScene); });
        Hide();
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
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

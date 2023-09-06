using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;

    private void Awake()
    {
        playBtn.onClick.AddListener(OnPlayClick);
        quitBtn.onClick.AddListener(OnQuitClick);
    }

    private void OnPlayClick()
    {
        SceneLoader.Load(SceneLoader.Scene.GameScene);
    }
    private void OnQuitClick()
    {
        Application.Quit();
    }
}

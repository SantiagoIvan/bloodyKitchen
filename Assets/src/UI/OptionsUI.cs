using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button musicBtn;
    [SerializeField] private Button specialEffectsBtn;
    [SerializeField] private Button backBtn;

    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI specialEffectsText;

    [SerializeField] private SoundManager soundManager;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private PauseUI pauseUI;

    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        musicBtn.onClick.AddListener(() => {
            musicManager.ChangeSound();
            UpdateVisual();

        });
        specialEffectsBtn.onClick.AddListener(() => { 
            soundManager.ChangeSound();
            UpdateVisual();
        });
        backBtn.onClick.AddListener(() =>
        {
            pauseUI.Show();
            Hide();
        });
        UpdateVisual();
        Hide();
    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void UpdateVisual()
    {
        // la escala es del 1 al 10.
        specialEffectsText.text = "SFX: " + Math.Floor(soundManager.GetCurrentVolume() * 10).ToString();
        musicText.text = "Music: " + Math.Floor(musicManager.GetCurrentVolume() * 10).ToString();
    }
}

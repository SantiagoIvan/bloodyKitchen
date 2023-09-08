using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button musicBtn;
    [SerializeField] private Button specialEffectsBtn;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI specialEffectsText;
    [SerializeField] private Button backBtn;

    // key bindings
    [SerializeField] private Button moveUpBtn;
    [SerializeField] private Button moveDownBtn;
    [SerializeField] private Button moveLeftBtn;
    [SerializeField] private Button moveRightBtn;
    [SerializeField] private Button interactBtn;
    [SerializeField] private Button useBtn;
    [SerializeField] private Button pauseBtn;

    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI useText;
    [SerializeField] private TextMeshProUGUI pauseText;



    [SerializeField] private SoundManager soundManager;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private PauseUI pauseUI;
    [SerializeField] private GameObject pressAnyKeyUI;

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

        moveUpBtn.onClick.AddListener(() => {
            RebindKey(GameInput.Binding.MOVE_UP);
        });
        moveDownBtn.onClick.AddListener(() => {
            RebindKey(GameInput.Binding.MOVE_DOWN);
        });
        moveLeftBtn.onClick.AddListener(() =>
        {
            RebindKey(GameInput.Binding.MOVE_LEFT);
        });
        moveRightBtn.onClick.AddListener(() => {
            RebindKey(GameInput.Binding.MOVE_RIGHT);
        });
        interactBtn.onClick.AddListener(() => {
            RebindKey(GameInput.Binding.INTERACT);
        });
        useBtn.onClick.AddListener(() =>
        {
            RebindKey(GameInput.Binding.USE);
        });
        pauseBtn.onClick.AddListener(() => {
            RebindKey(GameInput.Binding.PAUSE);
        });

        UpdateVisual();
        HidePressAnyKey();
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

        moveUpText.text = gameInput.GetBindingText(GameInput.Binding.MOVE_UP);
        moveDownText.text = gameInput.GetBindingText(GameInput.Binding.MOVE_DOWN);
        moveLeftText.text = gameInput.GetBindingText(GameInput.Binding.MOVE_LEFT);
        moveRightText.text = gameInput.GetBindingText(GameInput.Binding.MOVE_RIGHT);
        interactText.text = gameInput.GetBindingText(GameInput.Binding.INTERACT);
        useText.text = gameInput.GetBindingText(GameInput.Binding.USE);
        pauseText.text = gameInput.GetBindingText(GameInput.Binding.PAUSE);
    }

    private void ShowPressAnyKey()
    {
        pressAnyKeyUI.SetActive(true);
    }
    private void HidePressAnyKey()
    {
        pressAnyKeyUI.SetActive(false);
        UpdateVisual();
    }
    private void RebindKey(GameInput.Binding binding)
    {
        ShowPressAnyKey();
        gameInput.Rebind(binding, HidePressAnyKey);
    }
}

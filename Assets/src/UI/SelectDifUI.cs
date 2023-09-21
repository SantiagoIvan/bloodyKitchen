using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDifUI : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenu;
    [SerializeField] private Button easyBtn;
    [SerializeField] private Button hardBtn;
    [SerializeField] private Button backBtn;
    private const string DIFFICULTY_STRING = "Difficulty";
    public enum Difficulty
    {
        EASY,
        HARD
    }

    void Start()
    {
        easyBtn.onClick.AddListener(OnEasyBtnClick);
        hardBtn.onClick.AddListener(OnHardBtnClick);
        backBtn.onClick.AddListener(OnBackBtnClick);
        Hide();
    }

    private void OnEasyBtnClick()
    {
        PlayerPrefs.SetInt(DIFFICULTY_STRING, (int) Difficulty.EASY);
        PlayerPrefs.Save();
        SceneLoader.Load(SceneLoader.Scene.GameScene);
    }
    private void OnHardBtnClick()
    {
        PlayerPrefs.SetInt(DIFFICULTY_STRING, (int) Difficulty.HARD);
        PlayerPrefs.Save();
        SceneLoader.Load(SceneLoader.Scene.GameScene);
    }
    private void OnBackBtnClick()
    {
        mainMenu.Show();
        Hide();
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

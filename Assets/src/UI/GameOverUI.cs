using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ordersDeliveredNumber;
    [SerializeField] private TextMeshProUGUI uncompletedOrdersNumber;
    private GameManager gameManager;
    [SerializeField] private DeliveryManager deliveryManager;
    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnGameOver += GameManager_OnGameOver;
        playBtn.onClick.AddListener(() => { SceneLoader.Load(SceneLoader.Scene.GameScene); });
        quitBtn.onClick.AddListener(() => { SceneLoader.Load(SceneLoader.Scene.MainMenuScene); });
        Hide();
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        if (gameManager.IsGameOver())
        {
            Show();
            ordersDeliveredNumber.text = deliveryManager.GetOrdersDelivered().ToString();
            uncompletedOrdersNumber.text = deliveryManager.GetUncompletedOrders().ToString();
        }
    }
    private void Update()
    {
        if(!gameManager.IsGameOver()) { Hide(); }
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
    }
    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private DeliveryManager deliveryManager;
    [SerializeField] private Image img;
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite failedSprite;
    [SerializeField] private TextMeshProUGUI profitText;
    [SerializeField] private Animator animator;
    private const string POPUP = "PopUp";


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        deliveryManager.OnOrderCompleted += DeliveryManager_OnOrderCompleted;
        deliveryManager.OnWrongPlateDelivered += DeliveryManager_OnWrongPlateDelivered;

        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnWrongPlateDelivered(object sender, System.EventArgs e)
    {
        ShowCrossImage();
    }

    private void DeliveryManager_OnOrderCompleted(object sender, DeliveryManager.OnOrderCompletedEventArgs e)
    {
        ShowSuccessProfit(e.order);
    }
    private void ShowSuccessProfit(Order order)
    {
        profitText.text = "+ " + order.GetPrice().ToString();
        img.gameObject.SetActive(false);
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
    }
    private void ShowCrossImage()
    {
        gameObject.SetActive(true);
        img.gameObject.SetActive(true);
        profitText.text = "";
        img.sprite = failedSprite;
        img.color = new Color(failedColor.r, failedColor.g, failedColor.b);
        animator.SetTrigger(POPUP);
    }
}

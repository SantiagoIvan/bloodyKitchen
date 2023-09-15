using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private DeliveryManager deliveryManager;
    [SerializeField] private Image img;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failedSprite;
    [SerializeField] private Animator animator;
    private const string POPUP = "PopUp";

    private bool delivered;
    private float timer = 0;
    private const float LIMIT_TIMER = 1f;

    private void Awake()
    {
        delivered = false;
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
        ShowTickImage();
    }
    private void ShowTickImage()
    {
        gameObject.SetActive(true);
        //animator.SetTrigger(POPUP);
        img.sprite = successSprite;
        img.color = successColor;
    }
    private void ShowCrossImage()
    {
        gameObject.SetActive(true);
        //animator.SetTrigger(POPUP);
        img.sprite = failedSprite;
        img.color = failedColor;
    }
}

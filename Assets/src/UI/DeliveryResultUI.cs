using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private DeliveryManager deliveryManager;
    [SerializeField] private Image tickImage;
    [SerializeField] private Image crossImage;

    private void Awake()
    {
        tickImage.enabled = false;
        crossImage.enabled = false;
    }
    private void Start()
    {
        deliveryManager.OnOrderCompleted += DeliveryManager_OnOrderCompleted;
        deliveryManager.OnWrongPlateDelivered += DeliveryManager_OnWrongPlateDelivered;
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
        tickImage.enabled = true;
    }
    private void ShowCrossImage()
    {
        crossImage.enabled = true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderTemplateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI orderTitle;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;
    [SerializeField] private OrderTimer orderTimer;
    [SerializeField] private Animator animator;
    private const string CLOSE_TO_TIMEOUT = "CloseToTimeout";
    private int orderId;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetOrder(Order order)
    {
        orderTitle.text = order.GetRecipeName();
        orderTimer.SetOrderId(order.GetId());
        orderTimer.SetTimer(order.GetTimeout());
        orderTimer.OnCloseToTimeout += OrderTimer_OnCloseToTimeout;
        orderId = order.GetId();

        foreach (Transform child in iconContainer)
        {
            if (child != iconTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        // genero los ingredientes de la orden, las spawneo en el contenedor
        foreach (RecipeSO.Ingredient ingredient in order.GetRecipe().GetIngredients())
        {
            for (int i = 0; i < ingredient.amount; i++)
            {
                Transform spawned = Instantiate(iconTemplate, iconContainer);
                spawned.GetComponent<Image>().sprite = ingredient.typeIngredient.GetSprite();
                spawned.gameObject.SetActive(true);
            }
        }
        gameObject.SetActive(true);
    }

    private void OrderTimer_OnCloseToTimeout(object sender, EventArgs e)
    {
        animator.SetBool(CLOSE_TO_TIMEOUT, true);
    }

    public int GetOrderId() { 
        return orderId;
    }
}

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
    [SerializeField] private OrderTimerUI orderTimer;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetOrder(Order order)
    {
        orderTitle.text = order.GetRecipeName();
        orderTimer.SetTimer(order.GetTimeout());

        foreach (Transform child in iconContainer)
        {
            if (child != iconTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        // genero las recetas, las spawneo en el contenedor
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
}

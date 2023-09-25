using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    private RecipeSO recipe;
    private float timeout;
    private int id;

    public Order(int orderId, RecipeSO newRecipe, float t)
    {
        id = orderId;
        recipe = newRecipe;
        timeout = t;
    }

    public RecipeSO GetRecipe()
    {
        return recipe;
    }
    public float GetTimeout()
    {
        return timeout;
    }
    public string GetRecipeName()
    {
        return recipe.GetRecipeName();
    }
    public float GetPrice()
    {
        return recipe.GetPrice();
    }
    public int GetId()
    {
        return id;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    [SerializeField] private RecipesListSO recipes;
    public static OrderGenerator Instance { get; private set; }
    private const float MIN_TIMEOUT = 15f;
    private const float MAX_TIMEOUT = 25f;
    private int orderCount = 1;

    private void Awake()
    {
        Instance = this;
    }

    public Order GenerateNewOrder()
    {

        RecipeSO randomRecipe = recipes.GetRandomRecipeSO();
        System.Random rd = new System.Random();
        int generated = rd.Next((int) MIN_TIMEOUT, (int) MAX_TIMEOUT);
        Order newOrder = new Order(orderCount, randomRecipe, generated);
        orderCount++;
        return newOrder;
    }
}

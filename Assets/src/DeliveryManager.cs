using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

/* La idea es que este componente tenga la l�gica de las ordenes que van haciendo los clientes.
 * El Delivery Counter se deber�a comunicar con �ste para verificar si el plato entregado al ejecutar Interact() se corresponde
 * con algun pedido de alg�n cliente. A los clientes no los vemos f�sicamente pero imaginamos que nos llegan y que el Delivery Manager
 * recibe todos.
 * Por lo tanto, debe tener una Lista de pedidos, donde cada pedido es una lista de ingredientes (KitchenObjectSO)
 * Podr�a hacer que tenga una lista de lista, pero es muy poco expresivo, mejor crear una abstracci�n que puede ser Pedido o en este caso
 * como esta todo en ingl�s, Order
 
 */
public class DeliveryManager : MonoBehaviour
{
    private List<RecipeSO> orders;
    [SerializeField] private RecipesListSO recipes;

    [SerializeField] private float spawnTime = 1f; // en segundos
    private float currentTime;
    [SerializeField] private int maxOrders = 1;
    private int ordersCompleted = 0;
    private int wrongOrdersDelivered = 0;

    public event EventHandler<OnOrderSpawnedEventArgs> OnOrderSpawned;
    public event EventHandler<OnOrderCompletedEventArgs> OnOrderCompleted;
    public event EventHandler OnWrongPlateDelivered;
    public class OnOrderSpawnedEventArgs
    {
        public RecipeSO recipe;
    }
    public class OnOrderCompletedEventArgs
    {
        public RecipeSO recipe;
    }

    private void Awake()
    {
        orders = new List<RecipeSO>();
    }

    private void Update()
    {
        if(currentTime >= spawnTime && orders.Count < maxOrders)
        {
            SpawnOrder();
            currentTime = 0;
        }
        else
        {
            currentTime += Time.deltaTime;
        }
    }

    private void SpawnOrder()
    {
        RecipeSO newOrder = recipes.GetRandomRecipeSO();
        orders.Add(newOrder);
        OnOrderSpawned?.Invoke(this, new OnOrderSpawnedEventArgs { recipe = newOrder });
    }

    private bool IsPlateCorrect(PlateKitchenObject plate, out string recipeName)
    {
        for (int i = 0; i < orders.Count; i++)
        {
            if (orders[i].IsPlateCorrect(plate, out recipeName)) return true;
        }

        recipeName = null;
        return false;
    }

    private void OrderCompleted(string recipeName)
    {
        
        ordersCompleted++;
        RecipeSO target = recipes.GetRecipeSOByName(recipeName);
        orders.Remove(target);
        OnOrderCompleted?.Invoke(this, new OnOrderCompletedEventArgs { recipe = target });
    }
    public void DeliverPlate(PlateKitchenObject plate)
    {
        if(IsPlateCorrect(plate, out string recipeName)){
            OrderCompleted(recipeName);
            Debug.Log("Plate successfully delivered! " + recipeName);
        }
        else
        {
            wrongOrdersDelivered++;
            OnWrongPlateDelivered?.Invoke(this, EventArgs.Empty);
            Debug.Log("WRONG PLATE!");
        }
    }

    public List<RecipeSO> GetOrders(){ return orders; }
}

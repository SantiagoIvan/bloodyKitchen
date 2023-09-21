using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

/* La idea es que este componente tenga la lógica de las ordenes que van haciendo los clientes.
 * El Delivery Counter se debería comunicar con éste para verificar si el plato entregado al ejecutar Interact() se corresponde
 * con algun pedido de algún cliente. A los clientes no los vemos físicamente pero imaginamos que nos llegan y que el Delivery Manager
 * recibe todos.
 * Por lo tanto, debe tener una Lista de pedidos, donde cada pedido es una lista de ingredientes (KitchenObjectSO)
 * Podría hacer que tenga una lista de lista, pero es muy poco expresivo, mejor crear una abstracción que puede ser Pedido o en este caso
 * como esta todo en inglés, Order
 
 */
public class DeliveryManager : MonoBehaviour
{
    private List<RecipeSO> orders;
    [SerializeField] private RecipesListSO recipes;

    private float spawnTime;
    private float currentTime;
    private int maxOrders;
    private int ordersDelivered = 0;
    private int uncompletedOrders = 0;

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
    private void Start()
    {
        SpawnOrder();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if(currentTime >= spawnTime && orders.Count < maxOrders) // para que me lleguen ordenes cuando el juego empieza y no mientras leo el tutorial
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

        ordersDelivered++;
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
            uncompletedOrders++;
            OnWrongPlateDelivered?.Invoke(this, EventArgs.Empty);
            Debug.Log("WRONG PLATE!");
        }
    }

    public List<RecipeSO> GetOrders(){ return orders; }

    public int GetOrdersDelivered() { return ordersDelivered; }
    public int GetUncompletedOrders() { return uncompletedOrders; }
    public void SetMaxOrders(int q)
    {
        maxOrders = q;
    }
    public void SetSpawnTime(float q)
    {
        spawnTime = q;
    }
}

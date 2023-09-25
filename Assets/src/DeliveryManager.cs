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
    private List<Order> orders;
    [SerializeField] private RecipesListSO recipes;

    private float spawnTime;
    private float currentTime;
    private int maxOrders;
    private int ordersDelivered = 0;
    private float profit = 0;
    private float loss = 0;
    private int uncompletedOrders = 0;

    public event EventHandler<OnOrderSpawnedEventArgs> OnOrderSpawned;
    public event EventHandler<OnOrderCompletedEventArgs> OnOrderCompleted;
    public event EventHandler OnWrongPlateDelivered;
    public class OnOrderSpawnedEventArgs
    {
        public Order order;
    }
    public class OnOrderCompletedEventArgs
    {
        public Order order;
    }

    private void Awake()
    {
        orders = new List<Order>();
    }
    private void Start()
    {
        OrderTimer.OnOrderTimeout += OrderTimerUI_OnOrderTimeout;
    }

    private void OrderTimerUI_OnOrderTimeout(object sender, OrderTimer.OnOrderTimeOutEventArgs e)
    {
        OrderTimeOut(e.id);
        Debug.Log("TIMEOUT FROM MANAGER");
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
        Order orderSpawned = OrderGenerator.Instance.GenerateNewOrder();
        orders.Add(orderSpawned);
        Debug.Log("[LOG] ORDER_SPAWNED: "+orderSpawned.GetRecipe() +" - " + orderSpawned.GetTimeout().ToString());
        OnOrderSpawned?.Invoke(this, new OnOrderSpawnedEventArgs { order = orderSpawned });
    }

    private bool IsPlateCorrect(PlateKitchenObject plate, out int orderId)
    {
        for (int i = 0; i < orders.Count; i++)
        {
            if (orders[i].GetRecipe().IsPlateCorrect(plate))
            {
                orderId = orders[i].GetId();
                return true;
            }
        }
        orderId = -1;
        return false;
    }

    private void OrderCompleted(int orderId)
    {

        ordersDelivered++;
        Order ord = orders.Find( order => order.GetId() == orderId);
        profit += ord.GetPrice();
        orders.Remove(ord);
        OnOrderCompleted?.Invoke(this, new OnOrderCompletedEventArgs { order = ord });
    }
    public void DeliverPlate(PlateKitchenObject plate)
    {
        if(IsPlateCorrect(plate, out int orderId)){
            OrderCompleted(orderId);
            Debug.Log("Plate successfully delivered! Order ID: " + orderId.ToString());
        }
        else
        {
            uncompletedOrders++;
            OnWrongPlateDelivered?.Invoke(this, EventArgs.Empty);
            Debug.Log("WRONG PLATE!");
        }
    }

    public List<Order> GetOrders(){ return orders; }

    public int GetOrdersDelivered() { return ordersDelivered; }
    public int GetUncompletedOrders() { return uncompletedOrders + orders.Count; }
    public void SetMaxOrders(int q)
    {
        maxOrders = q;
    }
    public void SetSpawnTime(float q)
    {
        spawnTime = q;
    }
    public float GetProfit()
    {
        return profit;
    }
    public float GetLoss()
    {
        return loss + orders.Sum<Order>(order => order.GetRecipe().GetPrice());
    }
    public void OrderTimeOut(int orderId)
    {
        uncompletedOrders++;
        Order ord = orders.Find(order => order.GetId() == orderId);
        loss += ord.GetPrice();
        orders.Remove(ord);
    }
}

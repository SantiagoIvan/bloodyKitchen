using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    [SerializeField] private float spawnTime; // en segundos
    private float currentTime;
    [SerializeField] private int maxOrders;
    private int ordersCompleted;

    private void Start()
    {
        orders = new List<RecipeSO>();
        maxOrders = 4;
        spawnTime = 1f;
        ordersCompleted = 0;
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
    }

    public bool IsPlateCorrect(PlateKitchenObject plate, out string recipeName)
    {
        for (int i = 0; i < orders.Count; i++)
        {
            if (orders[i].IsPlateCorrect(plate, out recipeName)) return true;
        }

        recipeName = null;
        return false;
    }

    public void OrderCompleted(string recipeName)
    {
        
        ordersCompleted++;
        orders.Remove(recipes.GetRecipeSOByName(recipeName));
    }
}

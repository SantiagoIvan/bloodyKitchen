using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/* TODO Puedo disparar un efecto visual sobre la nueva orden que entró, como titilando. De esa forma, utilizaría el Argumento que le paso al Evento.
 * También puedo aplicarlo cuando una orden es completada y tengo que sacarlo de la lista, tipo un CHECK puedo hacer o algo asi
 */
public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] DeliveryManager deliveryManager;
    [SerializeField] private Transform listContainer;
    [SerializeField] private Transform orderTemplate;


    private void Awake()
    {
        orderTemplate.gameObject.SetActive(false); // desde la UI lo apago pero por las dudas lo hago aca tmb
    }


    // voy a crear un listener para saber cuando se creó una orden nueva y actualizar la UI
    private void Start()
    {
        deliveryManager.OnOrderSpawned += DeliveryManager_OnOrderSpawned;
        deliveryManager.OnOrderCompleted += DeliveryManager_OnOrderCompleted;
        UpdateVisual();
    }

    private void DeliveryManager_OnOrderCompleted(object sender, DeliveryManager.OnOrderCompletedEventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnOrderSpawned(object sender, DeliveryManager.OnOrderSpawnedEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        /* Misma idea que con los iconos del plato. Borro a todos y los vuelvo a crear, dejando al template solamente */
        foreach (Transform child in listContainer)
        {
            if (child != orderTemplate.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // genero las recetas, las spawneo en el contenedor
        foreach (RecipeSO order in deliveryManager.GetOrders())
        {
            Transform spawned = Instantiate(orderTemplate, listContainer);
            spawned.GetComponent<OrderTemplateUI>().SetRecipeSO(order);
            spawned.gameObject.SetActive(true);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        OrderTimer.OnOrderTimeout += OrderTimerUI_OnOrderTimeout;
    }

    private void OrderTimerUI_OnOrderTimeout(object sender, OrderTimer.OnOrderTimeOutEventArgs e)
    {
        RemoveOrderUI(e.id);
        Debug.Log("TIMEOUT FROM MANAGER UI");
    }

    private void DeliveryManager_OnOrderCompleted(object sender, DeliveryManager.OnOrderCompletedEventArgs e)
    {
        RemoveOrderUI(e.order.GetId());
    }

    private void DeliveryManager_OnOrderSpawned(object sender, DeliveryManager.OnOrderSpawnedEventArgs e)
    {
        // creo la orden visual
        Transform spawned = Instantiate(orderTemplate, listContainer);
        spawned.GetComponent<OrderTemplateUI>().SetOrder(e.order);
        spawned.gameObject.SetActive(true);
    }

    private void RemoveOrderUI(int orderTarget)
    {
        for(int i = 0; i < listContainer.childCount; i++)
        {
            Transform child = listContainer.GetChild(i);
            if(child != orderTemplate && child.GetComponent<OrderTemplateUI>().GetOrderId() == orderTarget)
            {
                Debug.Log("[CHILD]: I'M ORDER TARGET. ");
                Destroy(child.gameObject);
                break;
            }
            else
            {
                Debug.Log("[CHILD]: i'M NOT THE TARGET");
            }
        }
    }
}

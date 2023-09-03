using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    [SerializeField] private DeliveryManager deliveryManager;
    /* Solo va a aceptar platos que haya pedido alguien, es decir, que contenga el Delivery Manager*/
    public override void Interact()
    {
        PlayerController player = PlayerController.Instance;
        if (player.HasKitchenObject())
        {
            if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate) && deliveryManager.IsPlateCorrect(plate, out string recipeName))
            {
                player.GetKitchenObject().DestroySelf();
                deliveryManager.OrderCompleted(recipeName);
                Debug.Log("Plate successfully delivered! " + recipeName);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrashCounter : BaseCounter
{
    public override void Interact()
    {
        PlayerController player = PlayerController.Instance;
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
        }
    }
}

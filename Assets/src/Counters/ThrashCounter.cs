using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrashCounter : BaseCounter
{
    public static event EventHandler OnItemDropped;

    new public static void ResetStaticFields()
    {
        OnItemDropped = null;
    }
    public override void Interact()
    {
        PlayerController player = PlayerController.Instance;
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            OnItemDropped?.Invoke(this, EventArgs.Empty);
        }
    }
}

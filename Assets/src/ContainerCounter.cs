using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * En la parte visual del prefab, elimine del hijo "Selected" a la fotito del spawn porque no es necesario resaltar eso al acercarse el jugador, ya que se resalta toda la mesada.
 */
public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public EventHandler OnPlayerGrabbedUp;

    /* Por lo que creo yo, a  la primera interacción, si el jugador tiene las manos vacías, debería spawnearle en la mano el item que dropee esta mesada.
     */
    public override void Interact()
    {
        PlayerController player = PlayerController.Instance;

        if (!player.GetKitchenObject())
        {
            Transform kitchenObjectSpawned = Instantiate(kitchenObjectSO.GetPrefab());
            kitchenObjectSpawned.GetComponent<KitchenObject>().SetNewParent(player);
            OnPlayerGrabbedUp?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Debug.Log("The player already has an object!: " + player.GetKitchenObject().ToString());
        }

    }

    public KitchenObjectSO GetKitchenObjectSO() { return kitchenObjectSO; }
}

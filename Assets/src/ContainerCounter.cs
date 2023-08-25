using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform spawnPoint;
    private KitchenObject kitchenObject;

    /* Por lo que creo yo, a  la primera interacción, si el jugador tiene las manos vacías, debería spawnearle en la mano el item que dropee esta mesada.
     */
    public override void Interact()
    {
        PlayerController player = PlayerController.Instance;

        if (!player.GetKitchenObject())
        {
            Transform kitchenObjectSpawned = Instantiate(kitchenObjectSO.GetPrefab(), spawnPoint);
            kitchenObjectSpawned.GetComponent<KitchenObject>().SetNewParent(player);
        }
        else
        {
            Debug.Log("The player already has an object!: " + player.GetKitchenObject().ToString());
        }

    }
    public Transform GetSpawnPoint() { return spawnPoint; }
    public void SetKitchenObject(KitchenObject ko) { kitchenObject = ko; }
    public KitchenObject GetKitchenObject() { return kitchenObject; }
    public void ClearKitchenObject() { kitchenObject = null; }
    public bool HasKitchenObject() { return kitchenObject != null; }
}

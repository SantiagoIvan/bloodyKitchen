using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;

/* clase genérica de cualquier objeto de cocina, ya sea tomate, queso o cuchillo y demás
 * Todos los objetos o van a estar en un ClearCounter o van a estar en la mano del jugador
 * Quiero spawnear un objeto y poder agarrarlo, y dsp dejarlo en otro lugar.
 * En esencia, como un objeto va a estar siempre ligado a alguien (ya sea Clear Counter o Player controller), 
 * siempre va a tener un padre. Y no me refiero a padre como Herencia de clases, sino como alguien que lo tiene como referencia.
 */
public class KitchenObject : MonoBehaviour
{
    private IKitchenObjectParent currentParent;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public KitchenObjectSO GetKitchenObjectSO() { return kitchenObjectSO; }
    public IKitchenObjectParent GetParent() { return currentParent; }
    public void SetNewParent(IKitchenObjectParent newParent) {
        if (newParent.HasKitchenObject())
        {
            Debug.LogError("That Parent already has a kitchen Object");
        }
        if (this.currentParent != null)
        {
            currentParent.ClearKitchenObject();
        }
        this.currentParent = newParent;
        this.currentParent.SetKitchenObject(this);
        transform.parent = this.currentParent.GetSpawnPoint();
        transform.localPosition = Vector3.zero;
    }
    public void DestroySelf()
    {
        currentParent.ClearKitchenObject();
        Destroy(gameObject);
    }
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO newKitchenObject, IKitchenObjectParent parent)
    {
        Transform spawned = Instantiate(newKitchenObject.GetPrefab());
        KitchenObject spawnedObject = spawned.GetComponent<KitchenObject>();
        spawnedObject.SetNewParent(parent);
        return spawnedObject;
    }
    public bool TryGetPlate(out PlateKitchenObject plate)
    {
        if(this is PlateKitchenObject)
        {
            plate = this as PlateKitchenObject; // es un casteo
            return true;
        }
        else
        {
            plate = null;
            return false;
        }
    }
}

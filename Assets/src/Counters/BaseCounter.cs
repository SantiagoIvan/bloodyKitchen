using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Para la parte visual, cre� el Prefab BaseCounter, y para sus derivados, use la opci�n de Prefab Variant, que te permite crear variaciones de ese prefab
 * Es como tener una clase padre y clases hijas. En el Padre pongo todo lo que tienen en com�n y despues voy diferenciando a cada una, y por supuesto, si se comportan
 * diferente, cada hijo con su script
 * El Base Counter Script lo creo para que el jugador pueda interactuar con cualquier tipo de Mesada, y que despues cada implementaci�n haga lo suyo:
 * - Si es un Container Counter, deber�a spawnearme en la mano un tomate o lo que sea
 * - Si es una mesada limpia, deber�a poder dejar un item
 * 
 * Adem�s, como los distintos tipos de mesada tienen diferente cantidad de "partes" para renderizar, se me rompi� el SelectedCounterVisual.cs. Antes soportaba solo 1, asi que modifiqu� esa propiedad
 * para que sea un array.
 * 
 */
public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnDrop;
    public static void ResetStaticFields()
    {
        OnDrop = null;
    }

    [SerializeField] protected Transform spawnPoint;
    protected KitchenObject kitchenObject;
    public virtual void Interact() { }
    public virtual void Use() {
        Debug.Log("cant use this object");
    }
    public Transform GetSpawnPoint() { return spawnPoint; }
    public void SetKitchenObject(KitchenObject ko) { kitchenObject = ko; }
    public KitchenObject GetKitchenObject() { return kitchenObject; }
    public void ClearKitchenObject() { kitchenObject = null; }
    public bool HasKitchenObject() { return kitchenObject != null; }
}

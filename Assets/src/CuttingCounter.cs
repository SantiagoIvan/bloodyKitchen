using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : ClearCounter
{
    [SerializeField] private KitchenObject sliced; // TODO borrar esto
    /*
     * Hay 2 situaciones que contemplar
     * 1 - Al interactuar, funciona como Clear Counter: podemos depositar el item o sacarlo de ahi
     * 2 - Al apretar otro boton como por ejemplo R, accionamos al objeto, en este caso, lo corta
     * 
     * Como al interactuar, funciona igual que el clear counter, lo hago heredar de éste
     * Cambia la segunda situacion. Para ello, tengo que agregar un nuevo input
     */
    public EventHandler OnCuttingActionTriggered;
    public override void Use()
    {
        if (HasKitchenObject())
        {
            /* Faltaria loa logica para saber si ese item lo puedo cortar en pedacitos
             * Para ello, es necesario destruir el objeto que se encuentra sobre la mesa y crear otro nuevo, la version "Sliced" o rebanado
             */
            kitchenObject.DestroySelf();
            Transform slicedKitchenObject = Instantiate(sliced.GetKitchenObjectSO().GetPrefab());
            slicedKitchenObject.GetComponent<KitchenObject>().SetNewParent(this);
            OnCuttingActionTriggered?.Invoke(this, EventArgs.Empty);
        }
    }


}

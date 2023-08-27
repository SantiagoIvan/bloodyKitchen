using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttingCounter : ClearCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeArray; // Teoricamente es un mapa. 
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
            try
            {
                KitchenObjectSO koso = kitchenObject.GetKitchenObjectSO();
                CuttingRecipeSO target = cuttingRecipeArray.First<CuttingRecipeSO>(recipe => recipe.GetKitchenObjectSOInput() == koso);
                kitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(target.GetKitchenObjectSOOutput(), this);
                OnCuttingActionTriggered?.Invoke(this, EventArgs.Empty);
            }catch (Exception ex)
            {
                Debug.Log("Can't cut that object");
            }
        }
    }


}

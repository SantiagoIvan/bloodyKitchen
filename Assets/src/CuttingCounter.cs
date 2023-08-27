using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeArray; // Teoricamente es un mapa, porque cada uno de estos items tiene un input y un output. 
    /*
     * Hay 2 situaciones que contemplar
     * 1 - Al interactuar, funciona como Clear Counter: podemos depositar el item o sacarlo de ahi
     * 2 - Al apretar otro boton como por ejemplo R, accionamos al objeto, en este caso, lo corta
     * 
     * Como al interactuar, funciona igual que el clear counter, lo hago heredar de éste
     * Cambia la segunda situacion. Para ello, tengo que agregar un nuevo input
     */
    [SerializeField] private int cuttingProgress = 0;

    public EventHandler OnCuttingActionTriggered;
    public override void Use()
    {
        try
        {
            if (HasKitchenObject() && HasOutputRecipe(kitchenObject.GetKitchenObjectSO())) // verifico esto de nuevo porque un objeto al ser cortado se convierte en otro, y este está sobre la mesada asi que podría apretar la tecla "Use" de nuevo
            {
                cuttingProgress++;
                CuttingRecipeSO target = GetOutputRecipe();
                OnCuttingActionTriggered?.Invoke(this, EventArgs.Empty);
                if(cuttingProgress == target.GetCuttingQuantityLimit())
                {
                    kitchenObject.DestroySelf();
                    KitchenObject.SpawnKitchenObject(target.GetKitchenObjectSOOutput(), this);
                    cuttingProgress = 0;
                }
                
            }
        } catch (Exception) { 
            Debug.Log("Exception: Can't cut that object");
            
        }
    }
    public override void Interact()
    {
        // Si no hay nada sobre la mesa && el jugador tiene algo en la mano && este objeto es "cortable" => lo deposita.
        PlayerController player = PlayerController.Instance;
        if (player.GetKitchenObject() && !HasKitchenObject() && HasOutputRecipe(player.GetKitchenObject().GetKitchenObjectSO()))
        {
            player.GetKitchenObject().SetNewParent(this);
            cuttingProgress = 0; // si cambio de objeto, quiero que se resetee el contador
        }
        else if (!player.GetKitchenObject() && HasKitchenObject())
        {
            // si ya hay un objeto sobre la mesada y el jugador tiene las manos vacías => lo agarra
            kitchenObject.SetNewParent(PlayerController.Instance);
        }
        else
        {
            Debug.Log("Can't interact with this!!!");
    }
    
}

    private CuttingRecipeSO GetOutputRecipe()
    {
        KitchenObjectSO koso = kitchenObject.GetKitchenObjectSO();
        return cuttingRecipeArray.First<CuttingRecipeSO>(recipe => recipe.GetKitchenObjectSOInput() == koso);
    }
    private bool HasOutputRecipe(KitchenObjectSO koso)
    {
        return cuttingRecipeArray.AsQueryable<CuttingRecipeSO>().Any(recipe => recipe.GetKitchenObjectSOInput() == koso);
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CuttingCounter;

/* Aca la idea es parecida, voy a tener unas recetas que me van a indicar cuales son los objetos INPUT que yo puedo meter y su transición
 * Otra cosa no voy a poder apoyar sobre este tipo de mesa
 * Voy a necesitar saber que de la carne cruda, al pasar un tiempo se transforma en carne cocida, y luego de otro tiempo se transforma en carne quemada.
 * Es la misma idea que la Cutting Counter, tengo una abstracción que almacena, como el registro de un mapa, el inpuit y su output correspondiente.
 */

// TODO REFACTOR PORQUE CUTTING COUNTER Y STOVE COUNTER FUNCIONAN PRACTICAMENTE IGUAL. Template method tal vez funcione. Que ambos hereden de una clase UtilityCounter
public class StoveCounter : BaseCounter
{
    [SerializeField] private float cookTimer;
    [SerializeField] private FryingRecipeSO[] fryingRecipes;

    public override void Interact()
    {
        PlayerController player = PlayerController.Instance;
        if (player.GetKitchenObject() && !HasKitchenObject() && HasOutputRecipe(player.GetKitchenObject().GetKitchenObjectSO()))
        {
            player.GetKitchenObject().SetNewParent(this);
        }
        else if (!player.GetKitchenObject() && HasKitchenObject())
        {
            kitchenObject.SetNewParent(PlayerController.Instance);
        }
        else
        {
            Debug.Log("Can't interact with this!!!");
        }
    }
    public override void Use()
    {
        try
        {
            if(HasKitchenObject() && HasOutputRecipe(kitchenObject.GetKitchenObjectSO()))
            {
                FryingRecipeSO target = fryingRecipes.First<FryingRecipeSO>(recipe => recipe.GetKitchenObjectSOInput() == kitchenObject.GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                ClearKitchenObject();
                KitchenObject.SpawnKitchenObject(target.GetKitchenObjectSOOutput(), this);
            }
        } catch(Exception e)
        {
            Debug.LogError("Error cooking: " + e.Message);
        }
    }

    private FryingRecipeSO GetOutputRecipe()
    {
        KitchenObjectSO koso = kitchenObject.GetKitchenObjectSO();
        return fryingRecipes.First<FryingRecipeSO>(recipe => recipe.GetKitchenObjectSOInput() == koso);
    }
    private bool HasOutputRecipe(KitchenObjectSO koso)
    {
        return fryingRecipes.AsQueryable<FryingRecipeSO>().Any(recipe => recipe.GetKitchenObjectSOInput() == koso);
    }
}

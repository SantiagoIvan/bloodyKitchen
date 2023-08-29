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

public class StoveCounter : BaseCounter
{
    //private Enum State
    //{
    //    IDLE,
    //    FRYING
    //}

    [SerializeField] private float cookTimer = 0;
    [SerializeField] private FryingRecipeSO[] fryingRecipes;

    private void Update()
    {
        if (HasKitchenObject() && HasRecipe(GetKitchenObject().GetKitchenObjectSO()))
        {
            cookTimer += Time.deltaTime;
            Debug.Log(cookTimer);
            if(cookTimer >= GetRecipe().GetTransitionTime())
            {
                cookTimer = 0;
                Fry();
            }
        }
    }
    /* Coroutines: te permite ejecutar una función y retrasar su ejecución cuanto quisieras. Se puede pausar y retomar su ejecución en el siguiente frame manteniendo los valores de las variables.
     Es como si tuvieras un cron ejecutandose a la par, otro thread.
    Para el tema del timer, se puede solucionar con coroutines o con timers con un float.
    */

    public override void Interact()
    {
        PlayerController player = PlayerController.Instance;
        if (player.GetKitchenObject() && !HasKitchenObject() && HasRecipe(player.GetKitchenObject().GetKitchenObjectSO()))
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
    public void Fry()
    {
        try
        {
            if(HasKitchenObject() && HasRecipe(kitchenObject.GetKitchenObjectSO()))
            {
                FryingRecipeSO target = GetRecipe();
                GetKitchenObject().DestroySelf();
                ClearKitchenObject();
                KitchenObject.SpawnKitchenObject(target.GetKitchenObjectSOOutput(), this);
            }
        } catch(Exception e)
        {
            Debug.LogError("Error cooking: " + e.Message);
        }
    }

    private FryingRecipeSO GetRecipe()
    {
        KitchenObjectSO koso = kitchenObject.GetKitchenObjectSO();
        if (HasRecipe(koso))
        {
            return fryingRecipes.First<FryingRecipeSO>(recipe => recipe.GetKitchenObjectSOInput() == koso);
            
        }else return null;
    }
    private bool HasRecipe(KitchenObjectSO koso)
    {
        return fryingRecipes.AsQueryable<FryingRecipeSO>().Any(recipe => recipe.GetKitchenObjectSOInput() == koso);
    }

    
}

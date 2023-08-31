using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttingCounter : BaseCounter, IObjectWithProgress
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

    /* Clear, Cutting && Stove Counters son mesadas donde se pueden combinar objetos al interactuar, si yo tengo un tomate cortado, puedo llevar el plato y combinarlo con esos tomates
     */
    [SerializeField] private int cuttingProgress = 0;

    public event EventHandler OnCuttingActionTriggered;

    public event EventHandler<IObjectWithProgress.OnProgressChangedEventArgs> OnProgressChanged; // Esto es porque esa clase correspondiente a los Args, esta dentro de la Interfaz. Si yo creo otra igual aca adentro, va a ser diferente.

    public override void Use()
    {
        try
        {
            if (HasKitchenObject() && HasRecipe(kitchenObject.GetKitchenObjectSO())) // verifico esto de nuevo porque un objeto al ser cortado se convierte en otro, y este está sobre la mesada asi que podría apretar la tecla "Use" de nuevo
            {
                cuttingProgress++;
                CuttingRecipeSO target = GetRecipe();
                OnCuttingActionTriggered?.Invoke(this, EventArgs.Empty);
                OnProgressChanged?.Invoke(this, new IObjectWithProgress.OnProgressChangedEventArgs { currentProgress = (float) cuttingProgress / (float) target.GetCuttingQuantityLimit() });
                if(cuttingProgress >= target.GetCuttingQuantityLimit())
                {
                    kitchenObject.DestroySelf();
                    KitchenObject.SpawnKitchenObject(target.GetKitchenObjectSOOutput(), this);
                    cuttingProgress = 0;
                    OnProgressChanged?.Invoke(this, new IObjectWithProgress.OnProgressChangedEventArgs { currentProgress = (float) cuttingProgress });
                }
                
            }
        } catch (Exception) { 
            Debug.Log("Exception: Can't cut that object");
            
        }
    }
    public override void Interact()
    {
        try
        {
            // Si no hay nada sobre la mesa && el jugador tiene algo en la mano && este objeto es "cortable" => lo deposita.
            PlayerController player = PlayerController.Instance;
            if (player.GetKitchenObject() && !HasKitchenObject() && HasRecipe(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                player.GetKitchenObject().SetNewParent(this);
                cuttingProgress = 0; // si cambio de objeto, quiero que se resetee el contador
                OnProgressChanged?.Invoke(this, new IObjectWithProgress.OnProgressChangedEventArgs { currentProgress = cuttingProgress});
            }
            else if (!player.GetKitchenObject() && HasKitchenObject())
            {
                // si ya hay un objeto sobre la mesada y el jugador tiene las manos vacías => lo agarra
                kitchenObject.SetNewParent(PlayerController.Instance);
                OnProgressChanged?.Invoke(this, new IObjectWithProgress.OnProgressChangedEventArgs { currentProgress = 0f });
            }
            else if (HasKitchenObject() && player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate))
                {
                    // Intento combinarlo con lo que hay sobre la mesa
                    plate.TryAddIngredient(kitchenObject.GetKitchenObjectSO());
                    kitchenObject.DestroySelf();
                }
            }
        }catch (Exception e){
            Debug.LogException(e);
        }
    }

    private CuttingRecipeSO GetRecipe()
    {
        KitchenObjectSO koso = kitchenObject.GetKitchenObjectSO();
        return cuttingRecipeArray.First<CuttingRecipeSO>(recipe => recipe.GetKitchenObjectSOInput() == koso);
    }
    private bool HasRecipe(KitchenObjectSO koso)
    {
        return cuttingRecipeArray.AsQueryable<CuttingRecipeSO>().Any(recipe => recipe.GetKitchenObjectSOInput() == koso);
    }


}

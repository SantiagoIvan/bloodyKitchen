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

public class StoveCounter : BaseCounter, IObjectWithProgress
{
    public event EventHandler<IObjectWithProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public enum State
    {
        IDLE,
        FRYING,
        FRIED,
        BURNED
    }
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    [SerializeField] private float fryingTimer = 0;
    [SerializeField] private FryingRecipeSO[] fryingRecipes;
    private FryingRecipeSO currentRecipe;
    private State currentState;


    private void Start()
    {
        currentState = State.IDLE;
    }

    /* TODO separar esta logica de estados en diferentes clases, además puedo agregar efectos y sonidos en cada estado, si hago todo aca es un quilombo ademas de poco cohesivo. */
    private void Update()
    {
        switch (currentState)
        {
            case State.IDLE:
                break;
            case State.FRYING:
                fryingTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IObjectWithProgress.OnProgressChangedEventArgs { currentProgress = fryingTimer / currentRecipe.GetTransitionTime() });
                if(fryingTimer >= currentRecipe.GetTransitionTime())
                {
                    // ya ta frito como nuestra economia :'(
                    fryingTimer = 0;
                    Fried();
                    currentRecipe = GetRecipe();
                    currentState = State.FRIED;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = currentState});
                }
                break;
            case State.FRIED:
                fryingTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IObjectWithProgress.OnProgressChangedEventArgs { currentProgress = fryingTimer / currentRecipe.GetTransitionTime() });
                if (fryingTimer >= currentRecipe.GetTransitionTime())
                {
                    // se quemó
                    fryingTimer = 0;
                    Fried();
                    currentState = State.BURNED;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = currentState });
                }
                break;
            case State.BURNED:
                break;
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
            currentRecipe = GetRecipe();
            // Al poner algo, se cambia el estado de la cocina y se dispara el evento
            currentState = State.FRYING;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = currentState });

        }
        else if (!player.GetKitchenObject() && HasKitchenObject())
        {
            kitchenObject.SetNewParent(PlayerController.Instance);
            currentState = State.IDLE;
            fryingTimer = 0f;
            OnProgressChanged?.Invoke(this, new IObjectWithProgress.OnProgressChangedEventArgs { currentProgress = fryingTimer / currentRecipe.GetTransitionTime() });
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = currentState });
        }
        else
        {
            Debug.Log("Can't interact with this!!!");
        }
    }
    public void Fried()
    {
        try
        {
            GetKitchenObject().DestroySelf();
            ClearKitchenObject();
            KitchenObject.SpawnKitchenObject(currentRecipe.GetKitchenObjectSOOutput(), this);

        }
        catch (Exception e)
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
    private bool HasRecipe(KitchenObjectSO kosoInput)
    {
        return fryingRecipes.AsQueryable<FryingRecipeSO>().Any(recipe => recipe.GetKitchenObjectSOInput() == kosoInput);
    }

    
}

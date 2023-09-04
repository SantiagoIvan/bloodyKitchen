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


/* Clear, Cutting && Stove Counters son mesadas donde se pueden combinar objetos al interactuar, si yo tengo un tomate cortado, puedo llevar el plato y combinarlo con esos tomates
*/

public class StoveCounter : BaseCounter, IObjectWithProgress
{
    public event EventHandler OnStoveActive; // para los sonidos
    public event EventHandler OnStovePasive; // para los sonidos
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

    /* Es posible separar la logica esta en diferentes estado, pero la verdad como es bastante simple lo puedo dejar aca. */
    /* En cada cambio de estado tengo que emitir el evento para que actúe el sonido. Si paso a un estado Activo, emito OnStoveActive */
    private void Update()
    {
        switch (currentState)
        {
            case State.IDLE:
                break;
            case State.FRYING:
                fryingTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IObjectWithProgress.OnProgressChangedEventArgs { currentProgress = fryingTimer / currentRecipe.GetTransitionTime() });
                if (fryingTimer >= currentRecipe.GetTransitionTime())
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
                    // TODO emitir evento para que, al estar quemandose, se dispare un sonidito PII PII PII y ademas se cambie el efecto visual para que tire humo
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
            OnStoveActive?.Invoke(this, EventArgs.Empty);

        }
        else if (!player.GetKitchenObject() && HasKitchenObject())
        {
            /* Si el jugador no tiene nada en la mano y hay algo cocinandose => lo agarra */
            kitchenObject.SetNewParent(PlayerController.Instance);
            currentState = State.IDLE;
            fryingTimer = 0f;
            OnProgressChanged?.Invoke(this, new IObjectWithProgress.OnProgressChangedEventArgs { currentProgress = fryingTimer / currentRecipe.GetTransitionTime() });
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = currentState });
            OnStovePasive?.Invoke(this, EventArgs.Empty);
        }
        else if (HasKitchenObject() && player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate))
            {
                /* Si la mesa tiene un objeto cocinandose && el jugador tiene un objeto =>
                 * Si ese objeto es un plato, le doy el item que se está cocinando para que intente combinarlo y agregarlo al plato */
                plate.TryAddIngredient(kitchenObject.GetKitchenObjectSO());
                kitchenObject.DestroySelf();
                currentState = State.IDLE;
                fryingTimer = 0f;
                OnProgressChanged?.Invoke(this, new IObjectWithProgress.OnProgressChangedEventArgs { currentProgress = fryingTimer / currentRecipe.GetTransitionTime() });
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = State.IDLE });
                OnStovePasive?.Invoke(this, EventArgs.Empty);
            }
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

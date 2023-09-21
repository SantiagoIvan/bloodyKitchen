using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static OptionsUI;

public class GameInput : MonoBehaviour
{
    // LEGACY INPUT MANAGER
    // en cada frame se verifica qué tecla se presionó y se acciona
    // Update is called once per frame

    /**
     * GetKey sirve para saber si una tecla esta siendo presionada a lo largo del tiempo, como por ejemplo, cuando un personaje se mueve por el mapa
     * GeyKeyDown sirve para detectar si se presionó una tecla en un frame, como por ejemplo, saltar.

        Asi queda bien explicito que devuelve un vector unitario
        public Vector2 GetMovementNormalizedVector() 
{
        // Movimientos en el plano XZ. No es necesario un vector 3D para capturar el movimiento
        Vector2 inputVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = 1;
        }

        inputVector = inputVector.normalized; // esto es para que sea siempre un vector unitario. Si yo presiono 2 teclas, la magnitud debe seguir siendo 1.

        return inputVector;
        }
        */

    /* NEW INPUT SYSTEM
     * 
     *  para el tema del movimiento, es razonable chequearlo frame a frame, pero para las demas cosas como las interacciones, 
     *  como no son cosas que van a estar sucediendo constantemente, se utilizan los Eventos. Como es de costumbre,
     *  a la hora de suscribirme a un evento, le paso una función callback para que cuando suceda ese evento, llamen a esa función.
     *  
     *  Cuando se dispara el evento:
     *      OnInteractAction?.Invoke(this, EventArgs.Empty);
     *  - El primer argumento es la referencia a "quien disparó el evento"
     *  - El segundo es un Array de datos que pueden llegar a necesitar los suscriptores.
     *  - El tema del parentesis es similar al de Node Js. Si no es null, invoca a la función con esos argumentos.
     *  Es necesario tener suscriptores a la hora de disparar el evento.
     */
    public static GameInput Instance { get; private set; }
    private const string PLAYER_PREFS_INPUT_BINDINGS = "InputBindings";
    private PlayerInputActions inputActions;
    public event EventHandler OnInteractAction;
    public event EventHandler OnUseObjectAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    // en los cambios de escena, todos los gameOBjects se destruyen a menos que se diga lo contrario.
    // La clase PlayerInputActions creo que no lo hace, pero el tema es que si tiro pausa dsp de una partida tengo errores
    // porque intento referenciar algo que ya fue destruido asi que tengo que solucionarlo aca
    // otra cosa que no se borra y que persiste luego de los cambios de escena, son los campos STATIC asi que cuidado con eso porque puedo tener comportamiento no deseado
    // En este caso tengo al evento estatico del CuttingCounter para los sonidos. Si reseteo el juego voy a ver que los listeners de ese evento se van a ir sumando infinitamente, no se limpian
    // Lo cual puede surgir en errores porque si yo hago un debug dentro del listener, como las instancias viejas van a estar destruidas, va a tirar error.
    // GO TO ResetStaticFieldsManager


    public enum Binding
    {
        MOVE_UP,
        MOVE_DOWN,
        MOVE_LEFT,
        MOVE_RIGHT,
        INTERACT,
        USE,
        PAUSE
    }


    private void Awake()
    {
        Instance = this;
        inputActions = new PlayerInputActions();
        if (PlayerPrefs.HasKey(PLAYER_PREFS_INPUT_BINDINGS))
        {
            inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_INPUT_BINDINGS));
        }
        inputActions.Player.Enable(); // Se encuentra en Actions maps dentro de la configuración del PlayerInputActions

        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.UseObject.performed += UseObject_performed;
        inputActions.Player.Pause.performed += Pause_performed;

    }

    private void OnDestroy()
    {
        inputActions.Player.Interact.performed -= Interact_performed;
        inputActions.Player.UseObject.performed -= UseObject_performed;
        inputActions.Player.Pause.performed -= Pause_performed;

        inputActions.Dispose(); // memory free
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void UseObject_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnUseObjectAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementNormalizedVector()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized; // esta normalizacion se puede hacer tambien desde PlayerInputActions, en una sección llamada "Processors"
    }
    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            // para el caso de los movimientos, si me fijo en la configuración de PlayerInputActions, veo que estan todos en un array
            case Binding.MOVE_UP:
                return inputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.MOVE_DOWN:
                return inputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.MOVE_LEFT:
                return inputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.MOVE_RIGHT:
                return inputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.INTERACT:
                return inputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.USE:
                return inputActions.Player.UseObject.bindings[0].ToDisplayString();
            case Binding.PAUSE:
                return inputActions.Player.Pause.bindings[0].ToDisplayString();
        }
        return "";
    }

    public void Rebind(Binding binding, Action onRebound)
    {
        inputActions.Player.Disable();
        InputAction inputAction;
        int keyIndex;

        switch (binding)
        {
            default:
            // para el caso de los movimientos, si me fijo en la configuración de PlayerInputActions, veo que estan todos en un array
            case Binding.MOVE_UP:
                inputAction = inputActions.Player.Move;
                keyIndex = 1;
                break;
            case Binding.MOVE_DOWN:
                inputAction = inputActions.Player.Move;
                keyIndex = 2;
                break;
            case Binding.MOVE_LEFT:
                inputAction = inputActions.Player.Move;
                keyIndex = 3;
                break;
            case Binding.MOVE_RIGHT:
                inputAction = inputActions.Player.Move;
                keyIndex = 4;
                break;
            case Binding.INTERACT:
                inputAction = inputActions.Player.Interact;
                keyIndex = 0;
                break;
            case Binding.USE:
                inputAction = inputActions.Player.UseObject;
                keyIndex = 0;
                break;
            case Binding.PAUSE:
                inputAction = inputActions.Player.Pause;
                keyIndex = 0;
                break;
        }


        inputAction.PerformInteractiveRebinding(keyIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                inputActions.Player.Enable();
                onRebound();
                PlayerPrefs.SetString(PLAYER_PREFS_INPUT_BINDINGS, inputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            }).Start();
    }
}

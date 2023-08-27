using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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


    private PlayerInputActions inputActions;
    public event EventHandler OnInteractAction;
    public event EventHandler OnUseObjectAction;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable(); // Se encuentra en Actions maps dentro de la configuración del PlayerInputActions

        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.UseObject.performed += UseObject_performed;
    }

    private void UseObject_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("event triggered");
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
}

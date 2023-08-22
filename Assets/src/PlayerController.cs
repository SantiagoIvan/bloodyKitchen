using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private float rotationSpeed = 10f;
    private bool isWalking;
   

    // LEGACY INPUT MANAGER
    // en cada frame se verifica qué tecla se presionó y se acciona
    // Update is called once per frame

    /**
     * GetKey sirve para saber si una tecla esta siendo presionada a lo largo del tiempo, como por ejemplo, cuando un personaje se mueve por el mapa
     * GeyKeyDown sirve para detectar si se presionó una tecla en un frame, como por ejemplo, saltar.
     */
    private void Update()
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

        //modifico la posición del jugador. Como el jugador se mueve en un mapa de 3 dimensiones, es necesario hacer el casteo. Asi tengo separado el vector input, del movimiento en sí.
        Vector3 move = new Vector3(inputVector.x, 0, inputVector.y);
        transform.position +=  (Time.deltaTime * movementSpeed * move);
        // esta multiplicación se hace para que la velocidad ya no dependa de la cantidad de fps. Recordemos que Update se ejecuta en cada frame
        // Por lo tanto si tengo muchos mas fps, naturalmente la velocidad va a ser mayor. Pero si lo multiplico por un infinitesimal de tiempo, ese numero se mantiene
        // ya que si aumento los fps, el delta es aun más pequeño



        /**ROTACIÓN
         * Para manejar la rotación hacia la dirección donde me muevo, hay varias maneras
         * 1 - transform.rotation
         * 2 - eulerAngles
         * 3 - LookAt: rota pero dando la espalda. Si me muevo de forma circular, estaría siempre mirando hacia el centro y no hacia la dirección tangente
                   transform.LookAt(rotationSpeed * Time.deltaTime * move);
         * 4 - transform.forward
         */

        // transform.forward = move; si hago la rotación así, se logra pero de forma directa, no hay un suave movimiento hacia la rotación objetivo
        isWalking = move != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, move, Time.deltaTime * rotationSpeed);


    }

    public bool IsWalking()
    {
        return isWalking;
    }
}

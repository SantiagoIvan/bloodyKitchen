using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 1) En la UI de Unity se duplica el objeto y se lo llama "Selected". 
 * 2) Se hace 1% mas grande, cantidad suficiente para que se produzca la selección y se cambie el color de la copia, no haya un bug de texturas.
 * 3) Se le cambia el Material a la copia a uno especial que es como una telita, que da el efecto de transparencia
 * 4) Cuando el jugador se acerca a algo, envía un raycast y si cerca hay un Clear Counter, como "Selected" esta escondido por defecto, va a quedar seleccionado el original.
 * 5) Cada "Selected" tiene una referencia al Clear Counter original al cual pertenecen para saber si éste fue seleccionado y si es así, activarse.
 * 5) Al seleccionar un objeto, emite un evento, al cual todos los Clear Counter estan suscriptos
 * 6) Cada uno pregunta si ese nuevo clear Counter (que puede ser uno o null) corresponde con ellos, y si es así, activan el objeto Visual.
 */
public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField]
    private ClearCounter clearCounter;
    [SerializeField]
    private GameObject visualGameObject; // la mesada en si que voy a pintar al ser seleccionada. Lo que esta adentor del "Selected" del "Prefab"
    
    private void Start()
    {
        PlayerController.Instance.OnSelectedCounterChanged += PlayerController_OnSelectedCounterChanged;
    }

    private void PlayerController_OnSelectedCounterChanged(object sender, PlayerController.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedClearCounter == clearCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        visualGameObject.SetActive(true);
    }
    private void Hide()
    {
        visualGameObject.SetActive(false);
    }
}

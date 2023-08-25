using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * La animaci�n de la puertita que se abre y se cierra se dispara cada vez que el jugador interactuca con esta mesada.
 * Por lo tanto, en cada interacci�n, disparo el evento, y aca creo el listener y escucho. Cuando suceda, disparo la animaci�n
 */
public class ContainerCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";
    [SerializeField] private Animator animator;
    [SerializeField] private ContainerCounter containerCounter;
    [SerializeField] private GameObject door;

    private void Awake()
    {
        animator = GetComponent<Animator>(); // ya que es el mismo componente puedo hacer esto, sino tengo que hacer el Find y buscarlo por ID. No termina siendo flexible esa opci�n
    }
    private void Start()
    {
        containerCounter.OnPlayerGrabbedUp += ContainerCounter_OnPlayerGrabbedUp;
        door.GetComponent<SpriteRenderer>().sprite = containerCounter.GetKitchenObjectSO().GetSprite();
    }

    private void ContainerCounter_OnPlayerGrabbedUp(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}

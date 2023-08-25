using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    /* se lo puede poner como GameObject, es indistinto en este caso, investigar la diferencia entre ambos.
    [SerializeField] private Transform tomatoPrefab;
    La idea era que si yo queria spawnear un tomate al interactuar, tenia que tener esta referencia, linkear el TomatoPreFab y en la función Interact, instanciar el tomate
    Pero si quiero poder spawnear distintas cosas, esta solución no es flexible ya que voy a tener que empezar a agregar lineas, y encima no voy a poder saber que estoy spawneando 
    asi nomas, porque el Transform es una referencia ultra generica como GameObject, no voy a saber que esa referencia corresponde a un tomatoPrefab, voy a necesitar algo mas

    Entonces para solucionar esto hay algo llamado Scriptable Objects. Es una clase generica que guarda la referencia a un prefab, un nombre y otros datos. Es como aplicar POLIMORFISMO,
    ya que a muchos prefab los voy a hacer respetar un mismo contrato, que en este caso va a ser el KitchenObjects

    Para hacer spawnear al objeto encima de la mesada, creé el subGameObject SpawnPoint. En Unity lo acomodé para que esté centrado y encima de la mesada y lo linkié a este atributo.
    Entonces ahora, al interactuar, se instancia el prefab que esté dentor del KitchenObjectSO para que aparezca en el spawnPoint
    */

    

    /* Para el caso de las mesadas, lo que voy a querer hacer es dropear objetos en ellas. Lo que sea que tenga el jugador en la mano
     */
    public override void Interact()
    {
        // Si no hay nada sobre la mesa y el jugador tiene algo en la mano => lo deposita.
        PlayerController player = PlayerController.Instance;
        if (player.GetKitchenObject() && !kitchenObject)
        {
            player.GetKitchenObject().SetNewParent(this);
        }
        else if (!player.GetKitchenObject() && kitchenObject)
        {
            // si ya hay un objeto sobre la mesada y el jugador tiene las manos vacías => lo agarra
            kitchenObject.SetNewParent(PlayerController.Instance);
        }else
        {
            Debug.Log("Can't interact with this!!!");
        }
    }
}

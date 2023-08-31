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
        try
        {
            // Si no hay nada sobre la mesa y el jugador tiene algo en la mano => lo deposita.
            PlayerController player = PlayerController.Instance;
            if (player.GetKitchenObject() && !HasKitchenObject())
            {
                player.GetKitchenObject().SetNewParent(this);
            }
            else if (!player.GetKitchenObject() && HasKitchenObject())
            {
                // si ya hay un objeto sobre la mesada y el jugador tiene las manos vacías => lo agarra
                kitchenObject.SetNewParent(PlayerController.Instance);
            }else if (HasKitchenObject() && player.HasKitchenObject())
            {
                /* aca quiero ver si eso que tiene el jugador es un plato, para combinarlo con el objeto que tenga la mesa. Como el plato va a ser un objeto pero con algo de logica extra
                 * le creamos una clase y que herede de KitchenObjet, asi puede seguir siendo agarrado y además tiene la lógica extra de poder ponerle cosas encima (lista de KitchenObjects)
                 * Y que pueda encimarlos cumpliendo cierta regla. Por ejemplo, un pan nunca va a estar encima de la lechuga. Hademás hay objetos no combinables, como platos entre si.
                 * Otra cosa que hay que contemplar es el caso inverso: si el plato esta sobre la mesa y yo tengo por ejemplo la hamburguesa, quiero que se puedan combinar
                 */
            
                    if(player.GetKitchenObject() is PlateKitchenObject)
                    {
                        // Intento combinarlo con lo que hay sobre la mesa
                        PlateKitchenObject plate = player.GetKitchenObject() as PlateKitchenObject;
                        plate.AddIngredient(kitchenObject.GetKitchenObjectSO());
                        kitchenObject.DestroySelf();
                    }
                    else if (GetKitchenObject() is PlateKitchenObject)
                    {
                        // Intento combinarlo con lo que tiene el chabon
                        PlateKitchenObject plate = GetKitchenObject() as PlateKitchenObject;
                        plate.AddIngredient(player.GetKitchenObject().GetKitchenObjectSO());
                        player.GetKitchenObject().DestroySelf();
                    }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}

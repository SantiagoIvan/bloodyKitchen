using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

/* El plato es un subtipo de KitchenObject, ya que posee el mismo comportamiento (poder ser agarrado y todo eso) pero además tiene que tener esta logica de poder sumar ingredientes
 * y además tengo que tener una lista de ingredientes prohibidos para que no todo pueda combinarse (como 2 platos)
 * Por la logica que segui yo, los ingredientes prohibidos son
 * - Plato
 * - Lechuga sin cortar
 * - Queso sin cortar
 * - Tomate sin cortar
 * - Carne cruda
 * - Carne quemada
 */
public class PlateKitchenObject : KitchenObject
{
    /* TODO si quiero que los ingredientes se puedan repetir y no se cague la parte visual, voy a tener que agregarlos dinamicamente
    * Para ellos voy a tener que agregar un offset para cada ingrediente, y asi voy a saber cuanto se separan los panes en cada nuevo ingrediente que agrego.
    * De esta forma voy a poder armar patys con varias hamburguesas
    * En el tutorial, establece que solo puede haber un paty con esa combinación de ingredientes, donde cada uno tiene una sola ocurrencia
    * En mi juego, puede pasar cualquier cosa: podes tener 3 fetas de queso si queres, 3 patys.
    * 
    * Además, hay un prefab llamado PlateCompleteVisual, en los PrefabVisuals que da el chabon, donde tengo un paty con todos los prefab ingredientes que puede tener dentro.
    * Su idea es que al armar el plato, spawnee ese paty y vaya activando los prefab de los ingredientes que posee
    * Mi idea va a ser, armarlo con cualquier cantidad de cosas.
    */
    [SerializeField] private List<KitchenObjectSO> ingredients;

    [SerializeField] private List<KitchenObjectSO> forbiddenIngredients;
    public event EventHandler<OnNewIngredientEventArgs> OnNewIngredient;
    public class OnNewIngredientEventArgs
    {
        public KitchenObjectSO newIngredient;
    }
    /* Sería una lista de lis tipos de Objetos, no de los objetos en sí.
     * Si los objetos tuvieran stats, hp o atributos propios que valgan la pena, sería una lista de KitchenObjects (representarían a los objetos concretos). Además a partir del kitchenObject puedo obtener
     * el KitchenObjectSO y por ende el prefab y spawnear la parte visual.
     * Este caso es el mas simple, asi que solo es necesario el tipo y listo.
     */

    private void Awake()
    {
        ingredients = new List<KitchenObjectSO>();
    }
    public void TryAddIngredient(KitchenObjectSO ingredient)
    {
        if(!forbiddenIngredients.Contains(ingredient))
        {
            ingredients.Add(ingredient);
            OnNewIngredient?.Invoke(this, new OnNewIngredientEventArgs { newIngredient = ingredient });
        }
        else
        {
            throw new Exception("Forbidden ingredient!!");
        }
    }

    public List<KitchenObjectSO> GetIngredients() { return ingredients; }
}

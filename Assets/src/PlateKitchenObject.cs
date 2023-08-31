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
    [SerializeField] private float offset;
    [SerializeField] private Transform ingredientsSpawn;
    [SerializeField] private List<KitchenObjectSO> ingredients;

    [SerializeField] private List<KitchenObjectSO> forbiddenIngredients;
    /* Sería una lista de lis tipos de Objetos, no de los objetos en sí.
     * Si los objetos tuvieran stats, hp o atributos propios que valgan la pena, sería una lista de KitchenObjects (representarían a los objetos concretos). Además a partir del kitchenObject puedo obtener
     * el KitchenObjectSO y por ende el prefab y spawnear la parte visual.
     * Este caso es el mas simple, asi que solo es necesario el tipo y listo.
     */

    private void Awake()
    {
        ingredients = new List<KitchenObjectSO>();
        offset = 0.5f;
    }
    public void TryAddIngredient(KitchenObjectSO ingredient)
    {
        if(!forbiddenIngredients.Contains(ingredient))
        {
            Transform spawned = Instantiate(ingredient.GetPrefab(), ingredientsSpawn);
            spawned.localPosition = new Vector3(0, offset * ingredients.Count, 0);
            ingredients.Add(ingredient);
        }
        else
        {
            throw new Exception("Forbidden ingredient!!");
        }
    }
}

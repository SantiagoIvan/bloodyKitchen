using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


/* Ahora con esto puedo crear diferentes ordenes customizadas.
 */
[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    [SerializeField] private List<Ingredient> ingredients;
    [SerializeField] private string recipeName;
    [SerializeField] private float price;

    [Serializable]
    public struct Ingredient
    {
        public int amount;
        public KitchenObjectSO typeIngredient;
    }


    /* Para saber si un plato es correcto, tengo que ver que tanto en la receta como en el plato, cada ingrediente tenga la misma cantidad de ocurrencias.
    * Puedo armar un array de frecuencias para el plato y la receta y compararlos. Puedo modificar el RecipeSO para que en lugar de tener una lista simple, tenga una lista de 
    * structs con un KitchenObjectSO y la cantidad
    */
    public bool IsPlateCorrect(PlateKitchenObject plate, out string recipe)
    {
        Debug.Log("ANALYZING IF PLATE IS EQUAL TO " + recipeName);
        Dictionary<KitchenObjectSO, int> map = new Dictionary<KitchenObjectSO, int>();
        foreach(KitchenObjectSO koso in plate.GetIngredients())
        {
            map.TryGetValue(koso, out int value);
            if(value == 0)
            {
                map.Add(koso, 1);
            }
            else
            {
                map[koso] += 1;
            }
        }
        
        // si tienen la misma cantidad de entradas tanto el diccionario como la lista, y coinciden con los ingredientes y cantidades, es porque es el plato correcto
        if(map.Count == ingredients.Count) { 
            for(int i = 0; i < ingredients.Count; i++)
            {
                map.TryGetValue(ingredients[i].typeIngredient, out int quantity);
                if(quantity != ingredients[i].amount)
                {
                    recipe = null;
                    return false;
                }
            }
            recipe = recipeName;
            Debug.Log("Found a match!!! " + recipe);
            return true;
        }

        recipe = null;
        return false;
    }
    public string GetRecipeName()
    {
        return recipeName;
    }

    public List<Ingredient> GetIngredients() { return ingredients; }
    public float GetPrice()
    {
        return price;
    }
}

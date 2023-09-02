using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateParent;
    [SerializeField] private IconTemplate icon;

    private void Awake()
    {
        icon.gameObject.SetActive(false);
    }
    private void Start()
    {
        plateParent.OnNewIngredient += PlateParent_OnNewIngredient;
    }

    private void PlateParent_OnNewIngredient(object sender, PlateKitchenObject.OnNewIngredientEventArgs e)
    {
        // agregar un nuevo icono.
        UpdateVisual();
        for(int i = 0; i < plateParent.GetIngredients().Count; i++)
        {
            Debug.Log(plateParent.GetIngredients()[i].name);
        }
    }

    private void UpdateVisual()
    {
        /* Tengo 2 opciones para linkeas el Icon Template
         * 1- Lo convierto en hijo y lo escondo y hago el link. 
         *          - Tengo que hacer el if este para que no me lo rompa y asi poder instanciar
         *          - Es facil de ver que cierto objeto utiliza Icon Template para mostrar algo
         * 2- Lo borro de sus hijos y solamente utilizo el linkeo
         *          - Me ahorro el if dentro del primer for each
         *          
         * 
         * 
         * Voy a elegir la opción 1 porque siento que es mas facil de darse cuenta cual es el tipo de IconTemplate que usa.
        */
        foreach (Transform child in transform)
        {
            if (child != icon.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }
        foreach (KitchenObjectSO ingredient in plateParent.GetIngredients())
        {
            IconTemplate spawned = Instantiate(icon, transform);
            spawned.gameObject.SetActive(true);
            spawned.SetKitchenObjectSOImg(ingredient);
        }
    }
}

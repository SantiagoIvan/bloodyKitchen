using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObjectVisual : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateParent;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float currentIngredientsHeightVisual;
    [SerializeField] private float bottomBreadHeight;
    [SerializeField] private float bottomDistanceTopBread;

    [SerializeField] private KitchenObject bread;
    [SerializeField] private KitchenObjectSO breadSO;
    [SerializeField] private GameObject topBreadVisual;
    
    
    private void Start()
    {
        currentIngredientsHeightVisual = 0f;
        bottomBreadHeight = 0.17f;
        bottomDistanceTopBread = 0.12f;
        plateParent.OnNewIngredient += PlateParent_OnNewIngredient;
    }

    private void PlateParent_OnNewIngredient(object sender, PlateKitchenObject.OnNewIngredientEventArgs e)
    {
        /* Si es el pan, lo agrego abajo de todo y subo los elementos una cantidad igual a bottomBreadHeight.
         * Adem�s, el Top Bread tendr� que estar arriba de todo, a la altura de currentHeight pero sin modificar ese valor ya que si agrego cosas, no quiero que 
         * queden encima del pan
         */
        if(e.newIngredient == breadSO)
        {
            currentIngredientsHeightVisual += bottomBreadHeight;
            bread.gameObject.SetActive(true);
            // actualizo los hijos del spawn point, que es donde estan los ingredientes
            KitchenObject[] lista = spawnPoint.GetComponentsInChildren<KitchenObject>();
            foreach(KitchenObject ingredient in lista)
            {
                if(ingredient.GetKitchenObjectSO() != breadSO)
                {
                    ingredient.transform.SetLocalPositionAndRotation(new Vector3(0, ingredient.transform.localPosition.y + bottomBreadHeight,0), Quaternion.identity);
                }
            }
            updateTopBreadPosition();
        }
        else
        {
            Transform spawned = Instantiate(e.newIngredient.GetPrefab(), spawnPoint);
            spawned.localPosition = new Vector3(0, currentIngredientsHeightVisual, 0);
            currentIngredientsHeightVisual += e.newIngredient.GetVisualOffset();
            updateTopBreadPosition();
        }
    }
    private void updateTopBreadPosition()
    {
        topBreadVisual.transform.localPosition = new Vector3(0, currentIngredientsHeightVisual + bottomDistanceTopBread, 0);
    }
}

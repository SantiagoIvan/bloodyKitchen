using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObjectVisual : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateParent;
    private object offset;

    private void Start()
    {
        plateParent.OnNewIngredient += PlateParent_OnNewIngredient;
    }

    private void PlateParent_OnNewIngredient(object sender, PlateKitchenObject.OnNewIngredientEventArgs e)
    {
        Transform spawned = Instantiate(e.newIngredient.GetPrefab(), plateParent.GetIngredientSpawn());
        spawned.localPosition = new Vector3(0, plateParent.GetOffset() * (float) plateParent.GetIngredientsCount(), 0);
    }
}

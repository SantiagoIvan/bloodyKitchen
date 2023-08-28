using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Para saber qué receta spawneo cuando corto, la principal idea es básicamente un mapa
 * Tengo un array con estos objetos, donde cada abstracción almacena el tipo de entrada y de salida, asi se qué spawnear de acuerdo a lo que apoyo sobre la mesa
 */
[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{
    [SerializeField] private KitchenObjectSO kitchenObjectInput;
    [SerializeField] private KitchenObjectSO kitchenObjectOutput;
    [SerializeField] private int cuttingQuantityLimit;

    public KitchenObjectSO GetKitchenObjectSOInput() { return kitchenObjectInput; }
    public KitchenObjectSO GetKitchenObjectSOOutput() { return kitchenObjectOutput; }
    public int GetCuttingQuantityLimit() { return cuttingQuantityLimit; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Misma movida que con los Cutting Recipes. Cada subclase va a almacenar el input y el output, y otra información necesaria como el tiempo.
 */

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
    [SerializeField] private KitchenObjectSO kitchenObjectInput;
    [SerializeField] private KitchenObjectSO kitchenObjectOutput;
    [SerializeField] private float transitionTime;

    public KitchenObjectSO GetKitchenObjectSOInput() { return kitchenObjectInput; }
    public KitchenObjectSO GetKitchenObjectSOOutput() { return kitchenObjectOutput; }
    public float GetTransitionTime() { return transitionTime; }
}

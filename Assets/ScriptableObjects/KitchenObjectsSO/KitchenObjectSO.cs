using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()] // te crea una entrada en el menu de creación cuando haces click derecho.
public class KitchenObjectSO : ScriptableObject
{
    [SerializeField] private Transform prefab;
    [SerializeField] private Sprite sprite; // el modelo visual que va a renderizar
    [SerializeField] private string objectName;

    public Transform GetPrefab() { return prefab; }
    public string GetObjectName() { return objectName; }
    public Sprite GetSprite() { return sprite; }
}

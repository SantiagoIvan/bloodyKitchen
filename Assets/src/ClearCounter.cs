using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour
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

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform spawnPoint;
    private KitchenObject kitchenObject;
    public ClearCounter cc2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(kitchenObject != null)
            {
                kitchenObject.SetClearCounter(cc2);
                Debug.Log(kitchenObject.GetClearCounter());
                kitchenObject = null;
            }
        }
    }

    public void Interact()
    {
        if(kitchenObject == null)
        {
            Transform kitchenObjectSpawned = Instantiate(kitchenObjectSO.GetPrefab(), spawnPoint);
            kitchenObjectSpawned.localPosition = Vector3.zero; // posición local dentro del objeto, no local
            kitchenObject = kitchenObjectSpawned.GetComponent<KitchenObject>();
            kitchenObject.SetClearCounter(this);
        }
        else
        {
            Debug.Log(kitchenObject.GetClearCounter());
        }
    }

    public Transform GetSpawnPoint() { return spawnPoint; }
    public void SetKitchenObject(KitchenObject ko) { kitchenObject = ko; }
}

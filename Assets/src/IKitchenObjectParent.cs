using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Por como está diseñado el juego, un objeto solo puede estar en manos del jugador o en una mesada */
public interface IKitchenObjectParent
{
    public Transform GetSpawnPoint();
    public void SetKitchenObject(KitchenObject ko);
    public KitchenObject GetKitchenObject();
    public void ClearKitchenObject();
    public bool HasKitchenObject();
}

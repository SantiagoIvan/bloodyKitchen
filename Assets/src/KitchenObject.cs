using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* clase genérica de cualquier objeto de cocina, ya sea tomate, queso o cuchillo y demás
 * Todos los objetos o van a estar en un ClearCounter o van a estar en la mano del jugador
 * Quiero spawnear un objeto y poder agarrarlo, y dsp dejarlo en otro lugar.
 * En esencia, como un objeto va a estar siempre ligado a alguien (ya sea Clear Counter o Player controller), 
 * siempre va a tener un padre. Y no me refiero a padre como Herencia de clases, sino como alguien que lo tiene como referencia.
 */
public class KitchenObject : MonoBehaviour
{
    private ClearCounter clearCounter;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public KitchenObjectSO GetKitchenObjectSO() { return kitchenObjectSO; }
    public ClearCounter GetClearCounter() { return clearCounter; }
    public void SetClearCounter(ClearCounter cc) { 
        clearCounter = cc;
        //cc.SetKitchenObject(this);
        transform.parent = cc.GetSpawnPoint();
        transform.localPosition = Vector3.zero;
        Debug.Log("KO: new clear counter: " + clearCounter);
    }
}

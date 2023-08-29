using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* La idea es que spawnee un plato cada cierta cantidad de tiempo.
 * Como el juego estaba diseñado para que cada objeto pueda tener un padre, y cada padre pueda tener solo un objeto, la forma mas facil de
 hacer que este objeto posea varios es que visualmente aparezcan varios, y que por cada uno, se incremente un contador. Al interactuar con este objeto, si el contador
es positivo, agarrará un plato y sacará uno de los nuevos creados visualmente.*/
public class PlateCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateSO;
    private int plateQuantity;
    private int maxPlatesQuantity = 4; // para que no se haga una pila infinita de platos

    private float timer;
    private float spawnTime = 1f; // en segundos

    public EventHandler OnPlateSpawned;
    public EventHandler OnPlateTaken;


    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > spawnTime)
        {
            if(plateQuantity < maxPlatesQuantity)
            {
                plateQuantity++;
                // agregar uno al efecto visual. Disparo el evento asi el script que gestiona la parte visual se encarga
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
            timer = 0;
        }
    }

    public override void Interact()
    {
        PlayerController player = PlayerController.Instance;
        if(!player.HasKitchenObject() && plateQuantity > 0)
        {
            KitchenObject.SpawnKitchenObject(plateSO, player);
            plateQuantity--;
            OnPlateTaken?.Invoke(this, EventArgs.Empty);
        }
    }
}

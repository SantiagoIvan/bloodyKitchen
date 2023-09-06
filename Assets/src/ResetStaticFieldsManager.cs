using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// lo hacemos en el menu principal antes de empezar a jugar, es nuestra safe zone.
// cada clase que exponga algo estatico, tendra que tener una función de reset.
// podría hacer que cumplan con una interfaz y los voy iterando
public class ResetStaticFieldsManager : MonoBehaviour
{

    private void Awake()
    {
        ThrashCounter.ResetStaticFields();
        BaseCounter.ResetStaticFields();
        CuttingCounter.ResetStaticFields();
    }
}

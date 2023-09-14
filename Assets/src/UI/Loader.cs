using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    //al primer update, ya se carga la siguiente escena, pero en el "mientras tanto", queda congelada el loading screen y no el main menu.
    private void Update()
    {
        SceneLoader.LoaderCallback();
    }
}

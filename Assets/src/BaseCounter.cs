using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Para la parte visual, creé el Prefab BaseCounter, y para sus derivados, use la opción de Prefab Variant, que te permite crear variaciones de ese prefab
 * Es como tener una clase padre y clases hijas. En el Padre pongo todo lo que tienen en común y despues voy diferenciando a cada una, y por supuesto, si se comportan
 * diferente, cada hijo con su script
 * El Base Counter Script lo creo para que el jugador pueda interactuar con cualquier tipo de Mesada, y que despues cada implementación haga lo suyo:
 * - Si es un Container Counter, debería spawnearme en la mano un tomate o lo que sea
 * - Si es una mesada limpia, debería poder dejar un item
 */
public class BaseCounter : MonoBehaviour
{
    public virtual void Interact() { }
}

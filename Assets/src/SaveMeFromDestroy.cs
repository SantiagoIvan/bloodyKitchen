using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMeFromDestroy : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);    
    }
}

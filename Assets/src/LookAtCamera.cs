using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        Perpendicular
    }

    [SerializeField] private Mode mode;
    
    
    // corre despues del update normal
    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform); 
                break;
            case Mode.LookAtInverted:
                // tengo que calcular un vector desde este objeto (que seria donde esta esta clase, la barra de progreso) hacia la camara
                Vector3 direction = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + direction);
                break;
            case Mode.Perpendicular:
                transform.forward = Camera.main.transform.forward;
                break;
        }
    }
}

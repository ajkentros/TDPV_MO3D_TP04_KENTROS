using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMarker : MonoBehaviour
{
    public float cubeSize = 10f;

    void OnDrawGizmos()
    {
        // Mostrar el Gizmo solo si el GameObject está activo
        if (gameObject.activeSelf)
        {
            // Dibujar un cubo de color verde
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position, new Vector3(cubeSize, cubeSize, cubeSize));

            // Dibujar una línea roja en la dirección del forward del GameObject
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * cubeSize);
        }
    }
}


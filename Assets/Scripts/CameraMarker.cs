using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMarker : MonoBehaviour
{
    public float cubeSize = 10f;

    void OnDrawGizmos()
    {
        // Mostrar el Gizmo solo si el GameObject est� activo
        if (gameObject.activeSelf)
        {
            // Dibujar un cubo de color verde
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position, new Vector3(cubeSize, cubeSize, cubeSize));

            // Dibujar una l�nea roja en la direcci�n del forward del GameObject
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * cubeSize);
        }
    }
}


using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraConfigurator : MonoBehaviour
{
    // define variables cámara, texto de UI, slider para modificar parámetros: fov, size, nearCliep, farClip
    public Camera mainCamera;
    public TextMeshProUGUI projectionText;
    public Slider fovSlider;
    public Slider sizeSlider;
    public Slider nearClipSlider;
    public Slider farClipSlider;

    // define variable bool inicialmente en vista perspectiva
    private bool isPerspective = true; 

    private void Start()
    {
        // asegura que estén los componentes en la cámara
        if (mainCamera == null)
        {
            mainCamera = GetComponent<Camera>();
        }

        if (projectionText == null)
        {
            Debug.LogError("configurar el campo ProjectionText en la cámara");
        }

        if (fovSlider == null)
        {
            Debug.LogError("configurar el campo FOVSlider en la cámara");
        }

        if (sizeSlider == null)
        {
            Debug.LogError("configurar el campo SizeSlider en la cámara");
        }

        if (nearClipSlider == null)
        {
            Debug.LogError("configurar el campo NearClipSlider en la cámara");
        }

        if (farClipSlider == null)
        {
            Debug.LogError("configurar el campo FarClipSlider en la cámara");
        }

        // inicializa los valores de los controles con los valores actuales de la cámara y define valores mínimos y máximos según consigna
        fovSlider.value = mainCamera.fieldOfView;
        fovSlider.minValue = 0f;
        fovSlider.maxValue = 180f;
        //mainCamera.fieldOfView = 60f;    

        sizeSlider.value = mainCamera.orthographicSize;
        sizeSlider.minValue = 1f;
        sizeSlider.maxValue = 100f;

        nearClipSlider.value = mainCamera.nearClipPlane;
        nearClipSlider.minValue = 1f;
        nearClipSlider.maxValue = 100f;


        farClipSlider.value = mainCamera.farClipPlane;
        farClipSlider.minValue = 1f;
        farClipSlider.maxValue = 1000f;
    }

    // acciona el botón ToogleProyection
    public void ToggleProjection()
    {
        /*
        cambia el valor booleano al negado
        si isPerspective = on =>
            configura vista ortográfica = false
            resetea la proyección de la cámara
            cambia el texto en UI
            activa el slider fovSlider
            desactiva los slider: sizeSlider, nearClipSlider, farClipSlider
        sino =>
            configura vista ortográfica = true
            resetea la proyección de la cámara
            cambia el texto en UI
            desactiva el slider fovSlider
            activa los slider: sizeSlider, nearClipSlider, farClipSlider
         */
        isPerspective = !isPerspective;

        if (isPerspective)
        {
            mainCamera.orthographic = false;
            mainCamera.ResetProjectionMatrix();
            projectionText.text = "Perspective";

            // Activa el Slider en vista perspectiva
            fovSlider.interactable = true;
            sizeSlider.interactable = false;
            nearClipSlider.interactable = false;
            farClipSlider.interactable = false;
        }
        else
        {
            mainCamera.orthographic = true;
            mainCamera.orthographicSize = 5f; // ajusta el tamaño ortográfico según tus necesidades.
            projectionText.text = "Orthogonal";

            // Desactiva el Slider en vista ortogonal
            fovSlider.interactable = false;
            sizeSlider.interactable = true;
            nearClipSlider.interactable = true;
            farClipSlider.interactable = true;
        }
    }

    // actualiza el valor de fov
    public void UpdateFOV(float fovValue)
    {
        if (isPerspective)
        {
            mainCamera.fieldOfView = fovValue;
        }
    }

    // actualiza el valor de size
    public void UpdateSize(float sizeValue)
    {
        if (!isPerspective)
        {
            mainCamera.orthographicSize = sizeValue;
        }
    }

    // actualiza el valor de nearClip
    public void UpdateNearClip(float nearClipValue)
    {
        mainCamera.nearClipPlane = nearClipValue;
    }

    // actualiza el valor de farClip
    public void UpdateFarClip(float farClipValue)
    {
        mainCamera.farClipPlane = farClipValue;
    }

    // dibuja el gizmo de la cámara
    private void OnDrawGizmos()
    {
        // versión anterior
        //// dibuja el frustum de la cámara en la escena utilizando el aspect ratio actual
        //Gizmos.color = Color.yellow;

        //float aspectRatio = mainCamera.aspect;


        // dibuja la esfera roja en la posición de la cámara con un radio de 10 unidades
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 10f);

        // dibuja la línea roja desde el centro de la esfera en dirección al vector forward de la cámara
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10f);

        // configura la matriz de transformación local a mundial para solucionar el bug de Unity
        Gizmos.matrix = transform.localToWorldMatrix;

        // dibuja el frustum de la cámara en la escena utilizando el aspect ratio actual
        Gizmos.color = Color.yellow;

        // define la variable float aspectRadio = aspecto de la ManiCamera
        float aspectRatio = mainCamera.aspect;




        /*
        si la cámara está en vista perspectiva =>
            calcula la posición del gizmo de acuerdo a la posición de la cámara en modo perspectiva (toma el valor de fov) y luego dibuja el gizmo usando el método DrawFrustum de la clase Gizmos
        sino => 
            calcula la posición del gizmo de acuerdo a la posición de la cámara en modo ortográfico (toma el valor de size) y luego dibuja el gizmo usando el método DrawFrustum de la clase Gizmos
         */
        if (isPerspective)
        {
            
            float fov = mainCamera.fieldOfView;
            float distance = mainCamera.farClipPlane;

            float halfHeight = distance * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
            float halfWidth = halfHeight * aspectRatio;

            //Vector3 center = transform.position + transform.forward * distance; versión anterior

            Vector3 center = mainCamera.transform.position + mainCamera.transform.forward * distance;
            Gizmos.DrawFrustum(center, fov, distance, 0f, aspectRatio);
        }
        else
        {
            float size = mainCamera.orthographicSize;
            float halfHeight = size;
            float halfWidth = size * aspectRatio;

            // Vector3 center = transform.position + transform.forward * (mainCamera.nearClipPlane + size); versión anterior
            Vector3 center = mainCamera.transform.position + mainCamera.transform.forward * (mainCamera.nearClipPlane + size);
            Gizmos.DrawWireCube(center, new Vector3(halfWidth * 2f, halfHeight * 2f, size * 2f));
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraConfigurator : MonoBehaviour
{
    // define variables c�mara, texto de UI, slider para modificar par�metros: fov, size, nearCliep, farClip
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
        // asegura que est�n los componentes en la c�mara
        if (mainCamera == null)
        {
            mainCamera = GetComponent<Camera>();
        }

        if (projectionText == null)
        {
            Debug.LogError("configurar el campo ProjectionText en la c�mara");
        }

        if (fovSlider == null)
        {
            Debug.LogError("configurar el campo FOVSlider en la c�mara");
        }

        if (sizeSlider == null)
        {
            Debug.LogError("configurar el campo SizeSlider en la c�mara");
        }

        if (nearClipSlider == null)
        {
            Debug.LogError("configurar el campo NearClipSlider en la c�mara");
        }

        if (farClipSlider == null)
        {
            Debug.LogError("configurar el campo FarClipSlider en la c�mara");
        }

        // inicializa los valores de los controles con los valores actuales de la c�mara y define valores m�nimos y m�ximos seg�n consigna
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

    // acciona el bot�n ToogleProyection
    public void ToggleProjection()
    {
        /*
        cambia el valor booleano al negado
        si isPerspective = on =>
            configura vista ortogr�fica = false
            resetea la proyecci�n de la c�mara
            cambia el texto en UI
            activa el slider fovSlider
            desactiva los slider: sizeSlider, nearClipSlider, farClipSlider
        sino =>
            configura vista ortogr�fica = true
            resetea la proyecci�n de la c�mara
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
            mainCamera.orthographicSize = 5f; // ajusta el tama�o ortogr�fico seg�n tus necesidades.
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

    // dibuja el gizmo de la c�mara
    private void OnDrawGizmos()
    {
        // versi�n anterior
        //// dibuja el frustum de la c�mara en la escena utilizando el aspect ratio actual
        //Gizmos.color = Color.yellow;

        //float aspectRatio = mainCamera.aspect;


        // dibuja la esfera roja en la posici�n de la c�mara con un radio de 10 unidades
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 10f);

        // dibuja la l�nea roja desde el centro de la esfera en direcci�n al vector forward de la c�mara
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10f);

        // configura la matriz de transformaci�n local a mundial para solucionar el bug de Unity
        Gizmos.matrix = transform.localToWorldMatrix;

        // dibuja el frustum de la c�mara en la escena utilizando el aspect ratio actual
        Gizmos.color = Color.yellow;

        // define la variable float aspectRadio = aspecto de la ManiCamera
        float aspectRatio = mainCamera.aspect;




        /*
        si la c�mara est� en vista perspectiva =>
            calcula la posici�n del gizmo de acuerdo a la posici�n de la c�mara en modo perspectiva (toma el valor de fov) y luego dibuja el gizmo usando el m�todo DrawFrustum de la clase Gizmos
        sino => 
            calcula la posici�n del gizmo de acuerdo a la posici�n de la c�mara en modo ortogr�fico (toma el valor de size) y luego dibuja el gizmo usando el m�todo DrawFrustum de la clase Gizmos
         */
        if (isPerspective)
        {
            
            float fov = mainCamera.fieldOfView;
            float distance = mainCamera.farClipPlane;

            float halfHeight = distance * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
            float halfWidth = halfHeight * aspectRatio;

            //Vector3 center = transform.position + transform.forward * distance; versi�n anterior

            Vector3 center = mainCamera.transform.position + mainCamera.transform.forward * distance;
            Gizmos.DrawFrustum(center, fov, distance, 0f, aspectRatio);
        }
        else
        {
            float size = mainCamera.orthographicSize;
            float halfHeight = size;
            float halfWidth = size * aspectRatio;

            // Vector3 center = transform.position + transform.forward * (mainCamera.nearClipPlane + size); versi�n anterior
            Vector3 center = mainCamera.transform.position + mainCamera.transform.forward * (mainCamera.nearClipPlane + size);
            Gizmos.DrawWireCube(center, new Vector3(halfWidth * 2f, halfHeight * 2f, size * 2f));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraSwitch : MonoBehaviour
{
    // define dos variables p�blicas del tipo Camera para asociar a los GameObject MainCamera y SecundaryCamera
    public Camera mainCamera;
    public Camera secondaryCamera;

    // define variable del tipo Button para el boton 
    public Button switchCamera;

    // Variable del tipo TextMeshProUGUI para el texto del bot�n
    public TextMeshProUGUI buttonText;

    // Variable del tipo GameObject para el marcador de c�mara
    public GameObject cameraMarker;

    // Variable del tipo Button para el bot�n de posici�n
    public Button positionButton;

    // Variables para los marcadores de c�mara
    public GameObject cameraMarkerSur;
    public GameObject cameraMarkerNorte;
    public GameObject cameraMarkerEste;
    public GameObject cameraMarkerOeste;

    // Referencia al marcador de c�mara actual
    private GameObject currentCameraMarker;

    void Start()
    {
        // Asegurarse de que ambas c�maras est�n configuradas correctamente
        if (mainCamera == null || secondaryCamera == null)
        {
            Debug.LogError("Asignar las c�maras en el inspector");
            return;
        }

        // Asegurarse de que el bot�n est� configurado
        if (switchCamera != null)
        {
            
            // Obtener el componente TextMeshProUGUI del bot�n
            buttonText = switchCamera.GetComponentInChildren<TextMeshProUGUI>();

            // Agregar un listener al evento On Click del bot�n
            switchCamera.onClick.AddListener(SwitchCameras);
        }
        else
        {
            Debug.LogError("Asignar el bot�n SwitchCamera en el inspector");
        }

        // Agregar un listener al evento On Click del bot�n de posici�n
        if (positionButton != null)
        {
            positionButton.onClick.AddListener(ToggleCameraMarker);
        }
        else
        {
            Debug.LogError("Asigna el bot�n Position en el inspector.");
        }

        // Verificar si la c�mara secundaria est� activa al inicio
        if (secondaryCamera.gameObject.activeSelf)
        {
            // Mostrar el bot�n de posici�n
            ShowPositionButton(true);
        }
        else
        {
            // Ocultar el bot�n de posici�n
            ShowPositionButton(false);
        }

        // Inicializar las c�maras (solo una de ellas debe estar activa al principio)
        mainCamera.gameObject.SetActive(true);
        secondaryCamera.gameObject.SetActive(false);

        // Inicializar el texto del bot�n
        UpdateButtonText();

        // Establecer el marcador inicial como el marcador "sur"
        currentCameraMarker = cameraMarkerSur;
        MoveSecondaryCameraToMarker(currentCameraMarker.transform.position, currentCameraMarker.transform.rotation);
    }

    public void SwitchCameras()
    {
        // Cambiar el estado activo de las c�maras
        mainCamera.gameObject.SetActive(!mainCamera.gameObject.activeSelf);
        secondaryCamera.gameObject.SetActive(!secondaryCamera.gameObject.activeSelf);

        // Actualizar el texto del bot�n despu�s de cambiar las c�maras
        UpdateButtonText();

        // Mostrar u ocultar el bot�n de posici�n seg�n la c�mara activa
        ShowPositionButton(secondaryCamera.gameObject.activeSelf);
    }

    private void UpdateButtonText()
    {
        // Cambiar el texto del bot�n seg�n la c�mara activa
        string buttonTextString = mainCamera.gameObject.activeSelf ? "Secondary Camera" : "Main Camera";

        // Asignar el nuevo texto al componente TextMeshProUGUI
        if (buttonText != null)
        {
            buttonText.text = buttonTextString;
        }
        else
        {
            Debug.LogError("No se encontr� el componente TextMeshProUGUI en el bot�n");
        }
    }

    public void ShowPositionButton(bool show)
    {
        // Activar o desactivar el bot�n de posici�n
        if (positionButton != null)
        {
            positionButton.gameObject.SetActive(show);
        }
        else
        {
            Debug.LogError("Asigna el bot�n Position en el inspector.");
        }
    }

    public void ToggleCameraMarker()
    {
        // Activar o desactivar el marcador de c�mara al hacer clic en el bot�n Position
        if (currentCameraMarker != null)
        {
            currentCameraMarker.SetActive(!currentCameraMarker.activeSelf);
            MoveSecondaryCameraToMarker(currentCameraMarker.transform.position, currentCameraMarker.transform.rotation);
        }
        else
        {
            Debug.LogError("Asigna el marcador de c�mara en el inspector.");
        }

        // Cambiar al siguiente marcador en sentido antihorario
        SwitchToNextCameraMarker();
    }

    private void SwitchToNextCameraMarker()
    {
        if (currentCameraMarker == cameraMarkerSur)
        {
            currentCameraMarker = cameraMarkerOeste;
        }
        else if (currentCameraMarker == cameraMarkerOeste)
        {
            currentCameraMarker = cameraMarkerNorte;
        }
        else if (currentCameraMarker == cameraMarkerNorte)
        {
            currentCameraMarker = cameraMarkerEste;
        }
        else if (currentCameraMarker == cameraMarkerEste)
        {
            currentCameraMarker = cameraMarkerSur;
        }
    }

    private void MoveSecondaryCameraToMarker(Vector3 position, Quaternion rotation)
    {
        if (secondaryCamera != null)
        {
            // Mover y rotar la c�mara secundaria al marcador especificado
            secondaryCamera.transform.position = position;
            secondaryCamera.transform.rotation = rotation;
        }
    }
}

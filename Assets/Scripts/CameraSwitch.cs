using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraSwitch : MonoBehaviour
{
    // define dos variables públicas del tipo Camera para asociar a los GameObject MainCamera y SecundaryCamera
    public Camera mainCamera;
    public Camera secondaryCamera;

    // define variable del tipo Button para el boton 
    public Button switchCamera;

    // Variable del tipo TextMeshProUGUI para el texto del botón
    public TextMeshProUGUI buttonText;

    // Variable del tipo GameObject para el marcador de cámara
    public GameObject cameraMarker;

    // Variable del tipo Button para el botón de posición
    public Button positionButton;

    // Variables para los marcadores de cámara
    public GameObject cameraMarkerSur;
    public GameObject cameraMarkerNorte;
    public GameObject cameraMarkerEste;
    public GameObject cameraMarkerOeste;

    // Referencia al marcador de cámara actual
    private GameObject currentCameraMarker;

    void Start()
    {
        // Asegurarse de que ambas cámaras estén configuradas correctamente
        if (mainCamera == null || secondaryCamera == null)
        {
            Debug.LogError("Asignar las cámaras en el inspector");
            return;
        }

        // Asegurarse de que el botón esté configurado
        if (switchCamera != null)
        {
            
            // Obtener el componente TextMeshProUGUI del botón
            buttonText = switchCamera.GetComponentInChildren<TextMeshProUGUI>();

            // Agregar un listener al evento On Click del botón
            switchCamera.onClick.AddListener(SwitchCameras);
        }
        else
        {
            Debug.LogError("Asignar el botón SwitchCamera en el inspector");
        }

        // Agregar un listener al evento On Click del botón de posición
        if (positionButton != null)
        {
            positionButton.onClick.AddListener(ToggleCameraMarker);
        }
        else
        {
            Debug.LogError("Asigna el botón Position en el inspector.");
        }

        // Verificar si la cámara secundaria está activa al inicio
        if (secondaryCamera.gameObject.activeSelf)
        {
            // Mostrar el botón de posición
            ShowPositionButton(true);
        }
        else
        {
            // Ocultar el botón de posición
            ShowPositionButton(false);
        }

        // Inicializar las cámaras (solo una de ellas debe estar activa al principio)
        mainCamera.gameObject.SetActive(true);
        secondaryCamera.gameObject.SetActive(false);

        // Inicializar el texto del botón
        UpdateButtonText();

        // Establecer el marcador inicial como el marcador "sur"
        currentCameraMarker = cameraMarkerSur;
        MoveSecondaryCameraToMarker(currentCameraMarker.transform.position, currentCameraMarker.transform.rotation);
    }

    public void SwitchCameras()
    {
        // Cambiar el estado activo de las cámaras
        mainCamera.gameObject.SetActive(!mainCamera.gameObject.activeSelf);
        secondaryCamera.gameObject.SetActive(!secondaryCamera.gameObject.activeSelf);

        // Actualizar el texto del botón después de cambiar las cámaras
        UpdateButtonText();

        // Mostrar u ocultar el botón de posición según la cámara activa
        ShowPositionButton(secondaryCamera.gameObject.activeSelf);
    }

    private void UpdateButtonText()
    {
        // Cambiar el texto del botón según la cámara activa
        string buttonTextString = mainCamera.gameObject.activeSelf ? "Secondary Camera" : "Main Camera";

        // Asignar el nuevo texto al componente TextMeshProUGUI
        if (buttonText != null)
        {
            buttonText.text = buttonTextString;
        }
        else
        {
            Debug.LogError("No se encontró el componente TextMeshProUGUI en el botón");
        }
    }

    public void ShowPositionButton(bool show)
    {
        // Activar o desactivar el botón de posición
        if (positionButton != null)
        {
            positionButton.gameObject.SetActive(show);
        }
        else
        {
            Debug.LogError("Asigna el botón Position en el inspector.");
        }
    }

    public void ToggleCameraMarker()
    {
        // Activar o desactivar el marcador de cámara al hacer clic en el botón Position
        if (currentCameraMarker != null)
        {
            currentCameraMarker.SetActive(!currentCameraMarker.activeSelf);
            MoveSecondaryCameraToMarker(currentCameraMarker.transform.position, currentCameraMarker.transform.rotation);
        }
        else
        {
            Debug.LogError("Asigna el marcador de cámara en el inspector.");
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
            // Mover y rotar la cámara secundaria al marcador especificado
            secondaryCamera.transform.position = position;
            secondaryCamera.transform.rotation = rotation;
        }
    }
}

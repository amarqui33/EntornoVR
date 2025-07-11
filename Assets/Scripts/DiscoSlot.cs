using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DiscoSlot : MonoBehaviour
{
    public Transform posicionInsertada; // Posición del disco en el plato
    public EstadoGramofonoController fsm;

    private GameObject discoColocado;

    private void OnTriggerEnter(Collider other)
    {
        if (discoColocado != null) return;
        if (!other.CompareTag("Disco")) return;

        XRGrabInteractable discoGrab = other.GetComponent<XRGrabInteractable>();
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (discoGrab != null && discoGrab.isSelected)
        {
            var interactor = discoGrab.interactorsSelecting.Count > 0 ? discoGrab.interactorsSelecting[0] : null;
            if (interactor != null)
            {
                discoGrab.interactionManager.SelectExit(interactor, discoGrab);
            }
        }

        // Posiciona el disco en la posición insertada
        other.transform.position = posicionInsertada.position;
        other.transform.rotation = posicionInsertada.rotation;

        // Desactiva física e interacción
        if (rb != null) rb.isKinematic = true;
        if (discoGrab != null) discoGrab.enabled = false;

        discoColocado = other.gameObject;
        fsm.ColocarDisco(discoColocado);
        Debug.Log("💿 Disco colocado correctamente en el slot.");
    }
}

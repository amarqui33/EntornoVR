using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DiscoSlot : MonoBehaviour
{
    public Transform posicionInsertada; // posición final del disco
    public EstadoGramofonoController fsm;

    private void OnTriggerEnter(Collider other)
    {
        if (fsm.estadoActual != EstadoGramofono.Felt_Ready) return;

        if (other.CompareTag("Disco"))
        {
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

            // Posicionar disco
            other.transform.position = posicionInsertada.position;
            other.transform.rotation = posicionInsertada.rotation;

            // Desactivar física e interacción
            if (rb != null) rb.isKinematic = true;
            if (discoGrab != null) discoGrab.enabled = false;

            // Cambiar de estado
            fsm.ColocarDisco();
        }
    }
}
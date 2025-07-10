using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FieltroSlot : MonoBehaviour
{
    public Transform posicionInsertada; // posición final del fieltro
    public EstadoGramofonoController fsm;

    private void OnTriggerEnter(Collider other)
    {
        if (fsm.estadoActual != EstadoGramofono.Manivela_Ready) return;

        if (other.CompareTag("Fieltro"))
        {
            XRGrabInteractable fieltroGrab = other.GetComponent<XRGrabInteractable>();
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (fieltroGrab != null && fieltroGrab.isSelected)
            {
                var interactor = fieltroGrab.interactorsSelecting.Count > 0 ? fieltroGrab.interactorsSelecting[0] : null;
                if (interactor != null)
                {
                    fieltroGrab.interactionManager.SelectExit(interactor, fieltroGrab);
                }
            }

            // Posicionar fieltro
            other.transform.position = posicionInsertada.position;
            other.transform.rotation = posicionInsertada.rotation;

            // Desactivar física e interacción
            if (rb != null) rb.isKinematic = true;
            if (fieltroGrab != null) fieltroGrab.enabled = false;

            // Cambiar de estado
            fsm.ColocarFieltro();
        }
    }
}
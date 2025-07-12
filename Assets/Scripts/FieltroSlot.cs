using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FieltroSlot : MonoBehaviour
{
    public Transform posicionInsertada; // Posición final del fieltro
    public EstadoGramofonoController fsm;

    private GameObject fieltroColocado;
    private XRGrabInteractable fieltroGrab;
    private Rigidbody fieltroRb;

    private void OnTriggerEnter(Collider other)
    {
        if (fieltroColocado != null) return;
        if (fsm.estadoActual != EstadoGramofono.Manivela_Ready) return;
        if (!other.CompareTag("Fieltro")) return;

        fieltroGrab = other.GetComponent<XRGrabInteractable>();
        fieltroRb = other.GetComponent<Rigidbody>();

        if (fieltroGrab != null && fieltroGrab.isSelected)
        {
            var interactor = fieltroGrab.interactorsSelecting.Count > 0 ? fieltroGrab.interactorsSelecting[0] : null;
            if (interactor != null)
            {
                fieltroGrab.interactionManager.SelectExit(interactor, fieltroGrab);
            }
        }

        // Posiciona el fieltro
        other.transform.position = posicionInsertada.position;
        other.transform.rotation = posicionInsertada.rotation;

        // Desactiva física e interacción
        if (fieltroRb != null) fieltroRb.isKinematic = true;
        if (fieltroGrab != null) fieltroGrab.enabled = false;

        fieltroColocado = other.gameObject;
        fsm.ColocarFieltro();
        Debug.Log("🟣 Fieltro colocado correctamente.");
    }

    private void Update()
    {
        if (fieltroColocado != null && fsm.estadoActual == EstadoGramofono.Disk_Removed)
        {
            if (fieltroGrab != null && !fieltroGrab.enabled)
            {
                fieltroGrab.enabled = true;
                if (fieltroRb != null) fieltroRb.isKinematic = false;
                Debug.Log("✅ Fieltro habilitado para ser retirado.");
            }

            if (fieltroGrab != null && fieltroGrab.isSelected)
            {
                fsm.QuitarFieltro();
                GetComponent<Collider>().enabled = false; // Desactiva el slot
                Debug.Log("🟤 Fieltro retirado.");

                fieltroColocado = null;
                fieltroGrab = null;
                fieltroRb = null;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ManivelaSlot : MonoBehaviour
{
    public Transform posicionInsertada; // posición exacta donde quedará la manivela insertada
    public EstadoGramofonoController fsm; // referencia a la FSM para actualizar estado

    private XRGrabInteractable manivelaGrabInteractable;
    private Rigidbody manivelaRb;

    private bool manivelaInsertada = false;

    private void OnTriggerEnter(Collider other)
    {
        if (manivelaInsertada) return; // ya está insertada, no hacer nada

        if (other.CompareTag("Manivela"))
        {
            manivelaGrabInteractable = other.GetComponent<XRGrabInteractable>();
            manivelaRb = other.GetComponent<Rigidbody>();

            if (manivelaGrabInteractable != null && manivelaGrabInteractable.isSelected)
            {
                InsertarManivela();
            }
        }
    }

    private void InsertarManivela()
    {
        if (manivelaGrabInteractable == null) return;

        // Soltar la manivela del agarre del jugador
        var interactor = manivelaGrabInteractable.GetOldestInteractorSelecting();

        if (interactor != null)
        {
            manivelaGrabInteractable.interactionManager.SelectExit(interactor, manivelaGrabInteractable);
        }


        // Posicionar y alinear la manivela en el slot
        manivelaGrabInteractable.transform.position = posicionInsertada.position;
        manivelaGrabInteractable.transform.rotation = posicionInsertada.rotation;

        // Bloquear la manivela (no física ni interacción)
        if (manivelaRb != null)
        {
            manivelaRb.isKinematic = true;
            manivelaRb.useGravity = false;
        }
        manivelaGrabInteractable.enabled = false;

        manivelaInsertada = true;

        // Avisar a la FSM que la manivela está lista
        if (fsm != null)
        {
            fsm.estadoActual = EstadoGramofono.Manivela_Ready;
        }
    }

    // Método público para quitar la manivela (por ejemplo, llamado desde la FSM o UI)
    public void QuitarManivela()
    {
        if (!manivelaInsertada) return;

        // Habilitar interacción y física para poder agarrar la manivela otra vez
        if (manivelaGrabInteractable != null)
            manivelaGrabInteractable.enabled = true;

        if (manivelaRb != null)
        {
            manivelaRb.isKinematic = false;
            manivelaRb.useGravity = true;
        } 

        manivelaInsertada = false;

        // Puedes avisar a la FSM que la manivela ya no está insertada, si quieres
        if (fsm != null)
        {
            fsm.estadoActual = EstadoGramofono.Open_Empty;
        }
    }
}

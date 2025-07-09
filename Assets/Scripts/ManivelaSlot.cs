using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ManivelaSlot : MonoBehaviour
{
    public Transform posicionInsertada; // posici�n exacta donde quedar� la manivela insertada
    public EstadoGramofonoController fsm; // referencia a la FSM para actualizar estado

    private XRGrabInteractable manivelaGrabInteractable;
    private Rigidbody manivelaRb;

    private bool manivelaInsertada = false;

    private void OnTriggerEnter(Collider other)
    {
        if (manivelaInsertada) return; // ya est� insertada, no hacer nada

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

        // Bloquear la manivela (no f�sica ni interacci�n)
        if (manivelaRb != null)
        {
            manivelaRb.isKinematic = true;
            manivelaRb.useGravity = false;
        }
        manivelaGrabInteractable.enabled = false;

        manivelaInsertada = true;

        // Avisar a la FSM que la manivela est� lista
        if (fsm != null)
        {
            fsm.estadoActual = EstadoGramofono.Manivela_Ready;
        }
    }

    // M�todo p�blico para quitar la manivela (por ejemplo, llamado desde la FSM o UI)
    public void QuitarManivela()
    {
        if (!manivelaInsertada) return;

        // Habilitar interacci�n y f�sica para poder agarrar la manivela otra vez
        if (manivelaGrabInteractable != null)
            manivelaGrabInteractable.enabled = true;

        if (manivelaRb != null)
        {
            manivelaRb.isKinematic = false;
            manivelaRb.useGravity = true;
        } 

        manivelaInsertada = false;

        // Puedes avisar a la FSM que la manivela ya no est� insertada, si quieres
        if (fsm != null)
        {
            fsm.estadoActual = EstadoGramofono.Open_Empty;
        }
    }
}

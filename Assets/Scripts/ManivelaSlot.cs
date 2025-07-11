using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ManivelaSlot : MonoBehaviour
{
    public Transform posicionInsertada; // posición donde quedará la manivela insertada
    public EstadoGramofonoController fsm;

    private XRGrabInteractable manivelaGrabInteractable;
    private Rigidbody manivelaRb;

    private bool manivelaInsertada = false;

    private void OnTriggerEnter(Collider other)
    {
        if (manivelaInsertada) return;

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

        // Suelta la manivela del agarre
        var interactor = manivelaGrabInteractable.GetOldestInteractorSelecting();

        if (interactor != null)
        {
            manivelaGrabInteractable.interactionManager.SelectExit(interactor, manivelaGrabInteractable);
        }


        // Posicionaa y alineaa la manivela en el slot
        manivelaGrabInteractable.transform.position = posicionInsertada.position;
        manivelaGrabInteractable.transform.rotation = posicionInsertada.rotation;

        // Bloqueaa la manivela
        if (manivelaRb != null)
        {
            manivelaRb.isKinematic = true;
            manivelaRb.useGravity = false;
        }
        manivelaGrabInteractable.trackPosition = false; //no se meuve
        manivelaGrabInteractable.trackRotation = true;  //gira
        manivelaGrabInteractable.movementType = XRGrabInteractable.MovementType.Kinematic;



        manivelaInsertada = true;

        // Avisa a la FSM que la manivela está lista
        if (fsm != null)
        {
            fsm.InsertarManivela();
        }
    }
}

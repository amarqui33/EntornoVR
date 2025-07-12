using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ManivelaSlot : MonoBehaviour
{
    public Transform posicionInsertada;
    public EstadoGramofonoController fsm;

    private XRGrabInteractable manivelaGrab;
    private Rigidbody manivelaRb;

    private GameObject manivela;
    private bool manivelaInsertada = false;
    private bool listaParaQuitar = false;

    private void OnTriggerEnter(Collider other)
    {
        if (manivelaInsertada) return;
        if (!other.CompareTag("Manivela")) return;
        if (fsm.estadoActual != EstadoGramofono.Open_Empty &&
            fsm.estadoActual != EstadoGramofono.Manivela_Ready)
        {
            Debug.LogWarning("No puedes insertar la manivela en este estado.");
            return;
        }

        manivela = other.gameObject;
        manivelaGrab = manivela.GetComponent<XRGrabInteractable>();
        manivelaRb = manivela.GetComponent<Rigidbody>();

        if (manivelaGrab != null && manivelaGrab.isSelected)
        {
            InsertarManivela();
        }
    }

    private void InsertarManivela()
    {
        var interactor = manivelaGrab.GetOldestInteractorSelecting();
        if (interactor != null)
        {
            manivelaGrab.interactionManager.SelectExit(interactor, manivelaGrab);
        }

        manivela.transform.position = posicionInsertada.position;
        manivela.transform.rotation = posicionInsertada.rotation;

        if (manivelaRb != null)
        {
            manivelaRb.isKinematic = true;
            manivelaRb.useGravity = false;
        }

        manivelaGrab.trackPosition = false;
        manivelaGrab.trackRotation = true;
        manivelaGrab.movementType = XRGrabInteractable.MovementType.Kinematic;

        manivelaInsertada = true;
        listaParaQuitar = false;

        if (fsm != null)
        {
            fsm.InsertarManivela();
        }
    }

    private void Update()
    {
        // Espera hasta que pasamos a estado Felt_Removed para permitir que se pueda liberar
        if (manivelaInsertada && fsm.estadoActual == EstadoGramofono.Felt_Removed)
        {
            listaParaQuitar = true;
        }

        // Si está lista para quitar y el usuario la agarra
        if (listaParaQuitar && manivelaGrab != null && manivelaGrab.isSelected)
        {
            // Libera restricciones
            if (manivelaRb != null)
            {
                manivelaRb.isKinematic = false;
                manivelaRb.useGravity = true;
            }

            manivelaGrab.trackPosition = true;
            manivelaGrab.movementType = XRGrabInteractable.MovementType.VelocityTracking;

            // Cambia de estado
            fsm.QuitarManivela();

            // Actualiza flags
            manivelaInsertada = false;
            listaParaQuitar = false;

            Debug.Log("🛠Manivela retirada");
        }
    }
}

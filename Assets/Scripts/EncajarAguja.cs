using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EncajarAguja : MonoBehaviour
{
    public Transform posicionFinalAguja;
    public EstadoGramofonoController fsm;
    public AudioSource sonidoDisco;
    private bool colocada = false;

    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (colocada) return;

        if (other.CompareTag("TriggerAguja") && fsm.estadoActual == EstadoGramofono.Playing_Ready)
        {
            transform.position = posicionFinalAguja.position;
            transform.rotation = posicionFinalAguja.rotation;

            colocada = true;
            fsm.BajarAguja();

            sonidoDisco.Play();

            // Deshabilitar la interacción para que no se pueda mover más
            if (grabInteractable != null)
            {
                grabInteractable.enabled = false;
                // También puedes cancelar la interacción actual:
                grabInteractable.interactionManager?.CancelInteractableSelection((IXRSelectInteractable)grabInteractable);
            }

            Debug.Log("🎶 Aguja colocada: ¡Reproducción iniciada!");
        }
    }
}

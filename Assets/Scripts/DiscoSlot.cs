using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DiscoSlot : MonoBehaviour
{
    public EstadoGramofonoController fsm;
    private GameObject discoColocado;

    private void OnTriggerEnter(Collider other)
    {
        if (discoColocado != null) return;

        if (other.CompareTag("Disco"))
        {
            discoColocado = other.gameObject;

            // Coloca el disco en la posición del slot
            discoColocado.transform.position = transform.position;
            discoColocado.transform.rotation = transform.rotation;

            // Desactiva la interacción
            XRGrabInteractable grab = discoColocado.GetComponent<XRGrabInteractable>();
            if (grab != null)
            {
                grab.enabled = false;
                grab.interactionManager?.CancelInteractableSelection((IXRSelectInteractable)grab);
            }

            fsm.ColocarDisco(discoColocado);
            Debug.Log("💿 Disco colocado sobre el plato.");
        }
    }
}

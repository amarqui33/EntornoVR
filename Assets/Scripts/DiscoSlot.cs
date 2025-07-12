using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DiscoSlot : MonoBehaviour
{
    public Transform posicionInsertada;
    public EstadoGramofonoController fsm;

    private GameObject discoColocado;
    private XRGrabInteractable discoGrab;
    private Rigidbody discoRb;

    private void Update()
    {
        if (discoColocado != null && fsm.estadoActual == EstadoGramofono.Stopped)
        {
            if (discoGrab != null && !discoGrab.enabled)
            {
                discoGrab.enabled = true;
                if (discoRb != null) discoRb.isKinematic = false;
                Debug.Log("✅ Disco habilitado para ser retirado.");
            }

            if (discoGrab != null && discoGrab.isSelected)
            {
                fsm.QuitarDisco();

                // 🔴 Desactivar el slot para evitar recolocación automática
                GetComponent<Collider>().enabled = false;

                discoColocado = null;
                discoGrab = null;
                discoRb = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (discoColocado != null) return;
        if (!other.CompareTag("Disco")) return;

        discoGrab = other.GetComponent<XRGrabInteractable>();
        discoRb = other.GetComponent<Rigidbody>();

        if (discoGrab != null && discoGrab.isSelected)
        {
            var interactor = discoGrab.interactorsSelecting.Count > 0 ? discoGrab.interactorsSelecting[0] : null;
            if (interactor != null)
            {
                discoGrab.interactionManager.SelectExit(interactor, discoGrab);
            }
        }

        other.transform.position = posicionInsertada.position;
        other.transform.rotation = posicionInsertada.rotation;

        if (discoRb != null) discoRb.isKinematic = true;
        if (discoGrab != null) discoGrab.enabled = false;

        discoColocado = other.gameObject;
        fsm.ColocarDisco(discoColocado);
        Debug.Log("💿 Disco colocado correctamente en el slot.");
    }
}

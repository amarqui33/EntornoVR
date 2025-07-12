using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EncajarAguja : MonoBehaviour
{
    public Transform posicionFinalAguja;       // Posición fija sobre el disco
    public Transform posicionReposoBrazo;      // Posición original fuera del disco
    public EstadoGramofonoController fsm;
    public AudioSource sonidoDisco;
    public Transform extensionBrazo;


    private XRGrabInteractable grabInteractable;
    private bool reproduciendo = false;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerAguja") && fsm.estadoActual == EstadoGramofono.Playing_Ready && !reproduciendo)
        {
            // Coloca la aguja en la posición final
            transform.position = posicionFinalAguja.position;
            transform.rotation = posicionFinalAguja.rotation;

            // Cambia el estado y comienza reproducción
            fsm.BajarAguja();
            sonidoDisco.Play();

            reproduciendo = true;

            // Opcional: desactiva interacción para evitar moverla hasta que la agarren de nuevo
            if (grabInteractable != null)
            {
                grabInteractable.interactionManager?.CancelInteractableSelection((IXRSelectInteractable)grabInteractable);
                grabInteractable.enabled = false;
                Invoke(nameof(ActivarGrab), 0.1f); // Reactivar después de un frame para que funcione bien
            }

            Debug.Log("Aguja colocada sobre el disco. Reproducción iniciada.");
        }
    }

    private void ActivarGrab()
    {
        if (grabInteractable != null)
        {
            grabInteractable.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TriggerAguja") && reproduciendo && fsm.estadoActual == EstadoGramofono.Playing_Sound)
        {
            Debug.Log("Aguja retirada del disco. Reproducción detenida.");

            // Detener el sonido y cambiar estado
            sonidoDisco.Stop();
            fsm.SubirAguja();

            // Mover el brazo a la posición original
            StartCoroutine(VolverABrazoReposo());

            reproduciendo = false;
        }
    }

    private IEnumerator VolverABrazoReposo()
    {
        Vector3 startPos = extensionBrazo.position;
        Quaternion startRot = extensionBrazo.rotation;

        Vector3 targetPos = posicionReposoBrazo.position;
        Quaternion targetRot = posicionReposoBrazo.rotation;

        float duracion = 1f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duracion;
            extensionBrazo.position = Vector3.Lerp(startPos, targetPos, t);
            extensionBrazo.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        extensionBrazo.position = targetPos;
        extensionBrazo.rotation = targetRot;

        Debug.Log("Extensión del brazo volvió a la posición de reposo.");
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AgujaController : MonoBehaviour
{
    public Transform posicionFinalAguja;       // Posición fija sobre el disco
    public Transform posicionReposoBrazo;      // Posición original fuera del disco
    public EstadoGramofonoController fsm;
    public AudioSource sonidoDisco;

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
        float duracion = 1f;
        float t = 0f;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (t < duracion)
        {
            t += Time.deltaTime;
            float factor = Mathf.Clamp01(t / duracion);

            transform.position = Vector3.Lerp(startPos, posicionReposoBrazo.position, factor);
            transform.rotation = Quaternion.Slerp(startRot, posicionReposoBrazo.rotation, factor);

            yield return null;
        }

        transform.position = posicionReposoBrazo.position;
        transform.rotation = posicionReposoBrazo.rotation;
    }
}

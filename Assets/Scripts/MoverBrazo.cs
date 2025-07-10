using System.Collections;
using UnityEngine;
using UnityEngine.Animations; // para ParentConstraint
using UnityEngine.XR.Interaction.Toolkit;

public class MoverBrazo : MonoBehaviour
{
    public Transform brazoObjetivo;
    public float duracion = 1f;
    public EstadoGramofonoController fsm;

    private ParentConstraint constraint;
    private bool yaMovido = false;

    private void Awake()
    {
        constraint = GetComponent<ParentConstraint>();
        if (constraint == null)
        {
            Debug.LogWarning("No hay ParentConstraint en el brazo!");
        }
    }

    // Este método hay que llamarlo desde el evento onSelectEntered del XRSimpleInteractable en el inspector
    public void OnClickBrazo()
    {
        if (fsm.estadoActual == EstadoGramofono.Playing_Ready && !yaMovido)
        {
            Debug.Log("Click detectado en brazo");

            if (constraint != null)
            {
                constraint.constraintActive = false;
                constraint.enabled = false;
                Debug.Log("ParentConstraint desactivado");
            }
            else
            {
                Debug.LogWarning("Constraint no encontrado o ya desactivado");
            }

            StartCoroutine(Mover());
            yaMovido = true;
        }
    }

    private IEnumerator Mover()
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 targetPos = brazoObjetivo.position;
        Quaternion targetRot = brazoObjetivo.rotation;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duracion;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
            yield return null;
        }

        Debug.Log("Brazo movido a posición objetivo");
    }
}

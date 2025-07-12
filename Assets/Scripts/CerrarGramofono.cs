using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CerrarGramofono : MonoBehaviour
{
    public Animator animator;
    public EstadoGramofonoController fsm;
    public BrazoSigueTapa brazoScript;

    private XRBaseInteractable interactable;
    private bool yaCerrada = false;

    void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnSelect);
    }

    private void OnSelect(SelectEnterEventArgs args)
    {
        if (!yaCerrada && fsm != null && fsm.estadoActual == EstadoGramofono.Manivela_Removed)
        {
            if (brazoScript != null)
                brazoScript.ActivarConstraint();

            if (animator != null)
                animator.SetTrigger("Cerrar");

            yaCerrada = true;
            fsm.CerrarGramofono(); // cambia a Closed
            StartCoroutine(DesactivarConstraintTrasRetraso(1.5f)); // duracion animacion
        }
    }

    private IEnumerator DesactivarConstraintTrasRetraso(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        if (brazoScript != null)
            brazoScript.DesactivarConstraint();
    }

    void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnSelect);
        }
    }
}

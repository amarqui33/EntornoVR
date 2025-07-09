using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AbrirGramofono : MonoBehaviour
{
    public Animator animator;
    public EstadoGramofonoController fsm;  // Referencia a la FSM

    private XRBaseInteractable interactable;
    private bool yaAbierta = false;

    void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnSelect);
    }

    private void OnSelect(SelectEnterEventArgs args)
    {
        if (!yaAbierta && fsm != null && fsm.estadoActual == EstadoGramofono.Closed)
        {
            animator.SetTrigger("Abrir");
            yaAbierta = true;
            fsm.AbrirTapa();  // Avanza en la FSM
        }
    }

    void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnSelect);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AbrirGramofono : MonoBehaviour
{
    public Animator animator;
    public EstadoGramofonoController fsm;

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
            fsm.AbrirTapa();  //cambio de estado en maquina de estados
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

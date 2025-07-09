using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AbrirGramofono : MonoBehaviour
{
    public Animator animator;

    private XRBaseInteractable interactable;
    private bool yaAbierta = false;

    void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnSelect);
    }

    private void OnSelect(SelectEnterEventArgs args)
    {
        // Solo una vez
        if (!yaAbierta)
        {
            animator.SetTrigger("Abrir");
            yaAbierta = true;
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

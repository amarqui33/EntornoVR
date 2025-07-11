using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class DiscoSeleccionado : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private EstadoGramofonoController fsm;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        // Busca el controlador del gramófono en la escena (asumiendo que solo hay uno)
        fsm = FindObjectOfType<EstadoGramofonoController>();
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrabbed);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (fsm != null && fsm.dialogueUI != null)
        {
            var disco = GetComponent<Disco>();
            if (disco != null)
            {
                fsm.dialogueUI.MostrarDialogoPersonalizado($"Estás cogiendo el disco: {disco.nombreDisco}");
            }
        }
    }
}

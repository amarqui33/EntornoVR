using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DarCuerda : MonoBehaviour
{
    public EstadoGramofonoController fsm;
    public float gradosParaCuerda = 360f; // objetivo
    public Animator animator; // referencia al animator de la manivela

    private XRGrabInteractable grab;
    private float gradosAcumulados = 0f;
    private bool cuerdaDada = false;
    private IXRSelectInteractor interactorActual;
    private Quaternion rotacionInicial;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnSuelta);
    }

    private void OnDestroy()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnSuelta);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        interactorActual = args.interactorObject;
        rotacionInicial = interactorActual.transform.rotation;
        gradosAcumulados = 0f;  
        cuerdaDada = false;

        // Reiniciamos animación
        animator.Play("ManivelaGira", 0, 0f);
        animator.speed = 0f;
    }

    private void OnSuelta(SelectExitEventArgs args)
    {
        interactorActual = null;
        animator.speed = 0f; // Pausar animación al soltar
    }

    private void Update()
    {
        if (fsm.estadoActual != EstadoGramofono.Disk_Ready || cuerdaDada || interactorActual == null) return;

        Quaternion rotacionActual = interactorActual.transform.rotation;
        Quaternion delta = rotacionActual * Quaternion.Inverse(rotacionInicial);
        float deltaX = delta.eulerAngles.x;
        if (deltaX > 180f) deltaX -= 360f;

        gradosAcumulados += Mathf.Abs(deltaX);
        rotacionInicial = rotacionActual;

        // Avanzar la animación proporcional al progreso
        float progreso = Mathf.Clamp01(gradosAcumulados / gradosParaCuerda);
        animator.Play("ManivelaGira", 0, progreso);
        animator.speed = 0f; // fijamos manualmente el progreso

        if (gradosAcumulados >= gradosParaCuerda)
        {
            cuerdaDada = true;
            fsm.DarCuerda();
            Debug.Log("¡Cuerda dada!");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DarCuerda : MonoBehaviour
{
    public EstadoGramofonoController fsm;
    public Transform ejeRotacion;
    public float gradosParaCuerda = 360f;

    private XRGrabInteractable grab;
    private Quaternion rotacionInicial;
    private float gradosAcumulados = 0f;
    private bool cuerdaDada = false;
    private IXRSelectInteractor interactorActual;

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
    }

    private void OnSuelta(SelectExitEventArgs args)
    {
        interactorActual = null;
    }

    private void Update()
    {
        if (fsm.estadoActual != EstadoGramofono.Disk_Ready || cuerdaDada || interactorActual == null) return;

        Quaternion rotacionActual = interactorActual.transform.rotation;
        Quaternion deltaRotation = rotacionActual * Quaternion.Inverse(rotacionInicial);

        // Extraemos solo la rotación en el eje X
        float deltaX = deltaRotation.eulerAngles.x;
        if (deltaX > 180f) deltaX -= 360f;

        gradosAcumulados += Mathf.Abs(deltaX);
        rotacionInicial = rotacionActual;

        // Aplicar rotación al eje (solo en X)
        ejeRotacion.localRotation *= Quaternion.Euler(deltaX, 0f, 0f);

        if (gradosAcumulados >= gradosParaCuerda)
        {
            cuerdaDada = true;
            fsm.DarCuerda();
            Debug.Log("🎉 ¡Cuerda dada!");
        }

        Debug.Log("Giro acumulado: " + gradosAcumulados);
    }
}

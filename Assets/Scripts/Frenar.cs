using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Frenar : MonoBehaviour
{
    public EstadoGramofonoController fsm;
    public Animator animatorFreno;

    public List<GirarDisco> giradores = new List<GirarDisco>();

    private XRBaseInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnClickFreno);
    }

    private void OnDestroy()
    {
        interactable.selectEntered.RemoveListener(OnClickFreno);
    }

    private void OnClickFreno(SelectEnterEventArgs args)
    {
        if (fsm.estadoActual == EstadoGramofono.Wound)
        {
            // Estamos montando → quitar freno
            animatorFreno.SetTrigger("ActivarFreno");

            foreach (var g in giradores)
            {
                g.ActivarGiro(true);
            }

            fsm.QuitarFreno();
            Debug.Log("Freno quitado: Fieltro y vinilo girando...");
        }
        else if (fsm.estadoActual == EstadoGramofono.Playing_Ready)
        {
            // Estamos desmontando → poner freno
            // No lanzamos animación, solo aseguramos volver al estado Idle
            animatorFreno.Play("IdleFreno"); // Reinicia al estado inicial

            foreach (var g in giradores)
            {
                g.ActivarGiro(false); // Detener el giro
            }

            fsm.PonerFreno(); // Cambia a Stopped y lanza mensaje contextual
            Debug.Log("Freno puesto: plato detenido.");
        }
        else
        {
            Debug.LogWarning("No puedes usar el freno en este estado: " + fsm.estadoActual);
        }
    }
}


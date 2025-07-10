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
            animatorFreno.Play("QuitarFreno");

            // Activar giro en todos los componentes
            foreach (var g in giradores)
            {
                g.ActivarGiro(true);
            }

            fsm.QuitarFreno();
            Debug.Log("🎵 Freno quitado: Fieltro y vinilo girando...");
        }
        else
        {
            Debug.LogWarning("❌ No puedes quitar el freno en este estado: " + fsm.estadoActual);
        }
    }
}


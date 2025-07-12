using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoGramofonoController : MonoBehaviour
{   
    public EstadoGramofono estadoActual = EstadoGramofono.Closed;
    public DialogueUI dialogueUI;
    public AudioSource audioSource;
    private Disco discoActual;

    private void CambiarEstado(EstadoGramofono nuevoEstado)
    {
        EstadoGramofono estadoAnterior = estadoActual;
        estadoActual = nuevoEstado;

        if (dialogueUI != null)
            dialogueUI.MostrarDialogoConContexto(nuevoEstado.ToString(), estadoAnterior.ToString());
    }

    public void AbrirTapa()
    {
        if (estadoActual == EstadoGramofono.Closed)
        {
            Debug.Log("Tapa abierta");
            CambiarEstado(EstadoGramofono.Open_Empty);
        }
        else
        {
            Debug.LogWarning("No puedes abrir la tapa en este estado: " + estadoActual);
        }
    }

    public void InsertarManivela()
    {
        if (estadoActual == EstadoGramofono.Open_Empty)
        {
            Debug.Log("Manivela insertada");
            CambiarEstado(EstadoGramofono.Manivela_Ready);
        }
        else
        {
            Debug.LogWarning("No puedes insertar la manivela en este estado.");
        }
    }

    public void ColocarFieltro()
    {
        if (estadoActual == EstadoGramofono.Manivela_Ready)
        {
            Debug.Log("Fieltro colocado");
            CambiarEstado(EstadoGramofono.Felt_Ready);
        }
    }

    public void ColocarDisco(GameObject discoGO)
    {
        if (estadoActual == EstadoGramofono.Felt_Ready)
        {
            discoActual = discoGO.GetComponent<Disco>();
            if (discoActual != null)
            {
                Debug.Log("Disco colocado: " + discoActual.nombreDisco);
                dialogueUI.MostrarDialogoPersonalizado("Disco seleccionado: " + discoActual.nombreDisco);
            }

            CambiarEstado(EstadoGramofono.Disk_Ready);
        }
    }

    public void DarCuerda()
    {
        if (estadoActual == EstadoGramofono.Disk_Ready)
        {
            Debug.Log("Cuerda dada");
            CambiarEstado(EstadoGramofono.Wound);
        }
    }

    public void QuitarFreno()
    {
        if (estadoActual == EstadoGramofono.Wound)
        {
            Debug.Log("Freno quitado");
            CambiarEstado(EstadoGramofono.Playing_Ready);
        }
    }

    public void BajarAguja()
    {
        if (estadoActual == EstadoGramofono.Playing_Ready)
        {
            Debug.Log("Aguja bajada: empieza el sonido");
            CambiarEstado(EstadoGramofono.Playing_Sound);
        }
    }

    public void SubirAguja()
    {
        if (estadoActual == EstadoGramofono.Playing_Sound)
        {
            Debug.Log("Aguja levantada");
            CambiarEstado(EstadoGramofono.Playing_Ready);
        }
    }

    public void PonerFreno()
    {
        if (estadoActual == EstadoGramofono.Playing_Ready)
        {
            Debug.Log("Freno puesto: el plato se detiene");
            CambiarEstado(EstadoGramofono.Stopped);
        }
    }

    public void QuitarDisco()
    {
        if (estadoActual == EstadoGramofono.Stopped)
        {
            Debug.Log("Disco quitado");
            CambiarEstado(EstadoGramofono.Disk_Removed);
        }
    }

    public void QuitarFieltro()
    {
        if (estadoActual == EstadoGramofono.Disk_Removed)
        {
            Debug.Log("Fieltro quitado");
            CambiarEstado(EstadoGramofono.Felt_Removed);
        }
    }

    public void QuitarManivela()
    {
        if (estadoActual == EstadoGramofono.Felt_Removed)
        {
            Debug.Log("Manivela quitada");
            CambiarEstado(EstadoGramofono.Manivela_Removed);
        }
    }

    public void CerrarGramofono()
    {
        if (estadoActual == EstadoGramofono.Manivela_Removed)
        {
            Debug.Log("Gram�fono cerrado");
            CambiarEstado(EstadoGramofono.Closed);
        }
    }
    public EstadoGramofono ObtenerEstado()
    {
        return estadoActual;
    }
}

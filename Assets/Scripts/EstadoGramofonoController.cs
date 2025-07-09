using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoGramofonoController : MonoBehaviour
{   public EstadoGramofono estadoActual = EstadoGramofono.Closed;
    public void AbrirTapa()
    {
        if (estadoActual == EstadoGramofono.Closed)
        {
            Debug.Log("Tapa abierta");
            estadoActual = EstadoGramofono.Open_Empty;
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
            estadoActual = EstadoGramofono.Manivela_Ready;
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
            estadoActual = EstadoGramofono.Felt_Ready;
        }
    }

    public void ColocarDisco()
    {
        if (estadoActual == EstadoGramofono.Felt_Ready)
        {
            Debug.Log("Disco colocado");
            estadoActual = EstadoGramofono.Disk_Ready;
        }
    }

    public void DarCuerda()
    {
        if (estadoActual == EstadoGramofono.Disk_Ready)
        {
            Debug.Log("Cuerda dada");
            estadoActual = EstadoGramofono.Wound;
        }
    }

    public void QuitarFreno()
    {
        if (estadoActual == EstadoGramofono.Wound)
        {
            Debug.Log("Freno quitado");
            estadoActual = EstadoGramofono.Playing_Ready;
        }
    }

    public void BajarAguja()
    {
        if (estadoActual == EstadoGramofono.Playing_Ready)
        {
            Debug.Log("Aguja bajada: empieza el sonido");
            estadoActual = EstadoGramofono.Playing_Sound;
        }
    }

    public void SubirAguja()
    {
        if (estadoActual == EstadoGramofono.Playing_Sound)
        {
            Debug.Log("Aguja levantada");
            estadoActual = EstadoGramofono.Playing_Ready;
        }
    }

    public void PonerFreno()
    {
        if (estadoActual == EstadoGramofono.Playing_Ready)
        {
            Debug.Log("Freno puesto: el plato se detiene");
            estadoActual = EstadoGramofono.Stopped;
        }
    }

    public void Resetear()
    {
        if (estadoActual == EstadoGramofono.Stopped)
        {
            Debug.Log("Gramófono reseteado");
            estadoActual = EstadoGramofono.Open_Empty;
        }
    }

    public EstadoGramofono ObtenerEstado()
    {
        return estadoActual;
    }
}

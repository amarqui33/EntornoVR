using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public GameObject dialogueBox;
    public float textSpeed = 0.05f;
    public float tiempoVisible = 5f;

    private int index = 0;
    private string[] lines;

    void Start()
    {
        textComponent.text = string.Empty;
        dialogueBox.SetActive(false); // Oculta al inicio
    }

    // Llama a este método desde otro script para mostrar un diálogo según el estado
    public void MostrarDialogo(string estado)
    {
        lines = ObtenerTextoPorEstado(estado);
        if (lines.Length == 0) return;

        StopAllCoroutines(); // Por si hay otra animación de texto activa
        dialogueBox.SetActive(true);
        index = 0;
        StartCoroutine(MostrarLineas());
    }

    private string[] ObtenerTextoPorEstado(string estado)
    {
        switch (estado)
        {
            case "Closed":
                return new string[] { "Abre la tapa del gramófono." };
            case "Open_Empty":
                return new string[] { "Inserta la manivela." };
            case "Manivela_Ready":
                return new string[] { "Coloca el fieltro sobre el plato." };
            case "Felt_Ready":
                return new string[] { "Coloca el disco sobre el fieltro." };
            case "Disk_Ready":
                return new string[] { "Gira la manivela para dar cuerda." };
            case "Wound":
                return new string[] { "Quita el freno para empezar." };
            case "Playing_Ready":
                return new string[] { "Coloca la aguja en el disco." };
            case "Playing_Sound":
                return new string[] { "¡Reproduciendo música!" };
            case "Stopped":
                return new string[] { "El gramófono se ha detenido." };
            default:
                return new string[0]; // <- Asegura que siempre se devuelve algo
        }
    }


    private IEnumerator MostrarLineas()
    {
        while (index < lines.Length)
        {
            textComponent.text = "";
            yield return StartCoroutine(EscribirLinea(lines[index]));
            yield return new WaitForSeconds(tiempoVisible);
            index++;
        }

        dialogueBox.SetActive(false);
    }

    private IEnumerator EscribirLinea(string linea)
    {
        foreach (char c in linea.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}



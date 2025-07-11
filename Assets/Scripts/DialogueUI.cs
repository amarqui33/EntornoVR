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
        dialogueBox.SetActive(false); //oculta al inicio
    }

    // muestra un diálogo según el estado
    public void MostrarDialogo(string estado)
    {
        lines = ObtenerTextoPorEstado(estado);
        if (lines.Length == 0) return;

        StopAllCoroutines();
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
                return new string[] { "¡Felicidades! Has abierto el Gramófono. Ahora inserta la manivela en el lateral derecho de la base" };
            case "Manivela_Ready":
                return new string[] { "Has colocado la manivela en posición. Ahora debes colocar un fieltro sobre el plato" };
            case "Felt_Ready":
                return new string[] { "¡Genial! Escoge un disco y colocalo sobre el fieltro. ¡Tienes varios discos disponibles!" };
            case "Disk_Ready":
                return new string[] { "¡Bien hecho! Ahora gira la manivela hasta que tengamos suficiente cuerda" };
            case "Wound":
                return new string[] { "Ahora que la cuerda está dada debes quitar el freno" };
            case "Playing_Ready":
                return new string[] { "El plato está girando, para reproducir el audio debes colocar el brazo fonocaptor en posición y la aguja sobre el disco" };
            case "Playing_Sound":
                return new string[] { "¡Reproduciendo música!" };
            case "Stopped":
                return new string[] { "El gramófono se ha detenido" };
            default:
                return new string[0];
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

        public void MostrarDialogoPersonalizado(string mensaje)
    {
        StopAllCoroutines(); // Detiene cualquier diálogo anterior
        dialogueBox.SetActive(true); // Muestra la caja de diálogo
        textComponent.text = ""; // Limpia el texto actual
        StartCoroutine(EscribirLinea(mensaje)); // Escribe letra por letra el nuevo mensaje
        StartCoroutine(CerrarDialogoPasadoTiempo()); // Lo oculta tras X segundos
    }

    private IEnumerator CerrarDialogoPasadoTiempo()
    {
        yield return new WaitForSeconds(tiempoVisible);
        dialogueBox.SetActive(false);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirTapaConBrazo : MonoBehaviour
{
    public Animator animator;
    public BrazoSigueTapa brazoScript;

    public void AbrirTapa()
    {
        if (brazoScript != null)
            brazoScript.ActivarConstraint();

        if (animator != null)
            animator.SetTrigger("Abrir");

        // Opcional: desactivar el constraint tras un retardo
        StartCoroutine(DesactivarTrasRetraso(1.5f)); // ajusta el tiempo a la duración de la animación
    }

    private System.Collections.IEnumerator DesactivarTrasRetraso(float segundos)
    {
        yield return new WaitForSeconds(segundos);

        if (brazoScript != null)
            brazoScript.DesactivarConstraint();
    }
}

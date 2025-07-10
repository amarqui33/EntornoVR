using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Controla la animación de apertura de la tapa y coordinar cuándo el brazo debe seguirla
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
        StartCoroutine(DesactivarTrasRetraso(1.5f)); //tiempo duración animación
    }

    private System.Collections.IEnumerator DesactivarTrasRetraso(float segundos)
    {
        yield return new WaitForSeconds(segundos);

        if (brazoScript != null)
            brazoScript.DesactivarConstraint();
    }
}

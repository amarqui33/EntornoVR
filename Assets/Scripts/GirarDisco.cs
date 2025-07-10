using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirarDisco : MonoBehaviour
{
    public float velocidad = 100f; // grados por segundo
    public bool girando = false;

    void Update()
    {
        if (girando)
        {
            transform.Rotate(Vector3.up, velocidad * Time.deltaTime);
        }
    }

    public void ActivarGiro(bool estado)
    {
        girando = estado;
    }
}

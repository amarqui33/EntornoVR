using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BrazoSigueTapa : MonoBehaviour
{
    public ParentConstraint constraint;

    public void ActivarConstraint()
    {
        if (constraint != null)
            constraint.constraintActive = true;
    }

    public void DesactivarConstraint()
    {
        if (constraint != null)
            constraint.constraintActive = false;
    }
}

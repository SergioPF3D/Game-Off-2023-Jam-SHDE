using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : Activable
{
    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    Vector3 rotationDirection;

    [SerializeField]
    bool activated;

    float swich;
    public override void Activate()
    {
        activated = false;
    }
    public override void DeActivate()
    {
        activated = false;
    }

    private void FixedUpdate()
    {
        if (actualInputs > 0)
        {
            //Rota hacia un ladoS 
        }
        else if (actualInputs < 0)
        {
            //Rota hacia otro lado
        }
    }
}


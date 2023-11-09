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
            //Rota hacia un lado
            //Rota hacia otro lado
        }
    }
}

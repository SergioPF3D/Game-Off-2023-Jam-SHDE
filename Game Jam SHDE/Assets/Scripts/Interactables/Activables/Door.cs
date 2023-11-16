using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : ObjectThatMoves
{
    [Header("Door")]
    //Things to activate
    BoxCollider colider;
    MeshRenderer render;

   

    public override void Start()
    {
        colider = this.gameObject.GetComponent<BoxCollider>();
        render = this.gameObject.GetComponent<MeshRenderer>();

        base.Start();
    }

    public override void Activate()
    {
        StartCoroutine(MoveDoor(transform.position, finalPosition.position));
    }
    public override void DeActivate()
    {
        StartCoroutine(MoveDoor(transform.position, initialPosition.position));
    }

    public void seeIfActivate()
    {
        if (actualInputs >= inputs)
        {
            if (baseActivated)
            {
                DeActivate();
            }
            else
            {
                Activate();
            }
        }
        else
        {
            if (baseActivated)
            {
                Activate();
            }
            else
            {
                DeActivate();
            }
        }
    }

    public override void AddInput(float inputs)
    {
        base.AddInput(inputs);
        seeIfActivate();
    }
    public override void RemoveInput(float inputs)
    {
        base.RemoveInput(inputs);
        seeIfActivate();
    }

    
}

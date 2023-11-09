using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activable
{
    //Things to activate
    [HideInInspector]
    BoxCollider colider;
    [HideInInspector]
    MeshRenderer render;

    public override void Start()
    {
        colider = this.gameObject.GetComponent<BoxCollider>();
        render = this.gameObject.GetComponent<MeshRenderer>();

        base.Start();
    }

    public override void Activate()
    {
        colider.enabled = true;
        render.enabled = true;
    }
    public override void DeActivate()
    {
        colider.enabled = false;
        render.enabled = false;
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

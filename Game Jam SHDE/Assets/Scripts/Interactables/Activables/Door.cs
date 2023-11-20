using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : ObjectThatMoves
{
    [Header("Door")]
    //Things to activate
    BoxCollider colider;
    MeshRenderer render;

    List<GameObject> particles;

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
    public override IEnumerator MoveDoor(Vector3 position1, Vector3 position2)
    {
        ActivateParticles(true);
        base.MoveDoor(position1, position2);
        ActivateParticles(false);
        yield return null;
    }

    void ActivateParticles(bool active)
    {
        foreach (var particle in particles)
        {
            //particle.SetActive(active);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : ObjectThatMoves
{
    [Header("Door")]
    //Things to activate
    
    [SerializeField]
    List<GameObject> particles;

    public bool blocked;

    [SerializeField]
    AudioSource audios;

    [SerializeField]
    bool levelDoor;

    [SerializeField]
    AudioClip levelUp;
    public override void Start()
    {
        base.Start();
    }

    public override void Activate()
    {
        if (blocked || activated)
        {
            return;
        }
        StopAllCoroutines();
        StartCoroutine(MoveDoor(transform.position, finalPosition.position));
        base.Activate();
    }
    public override void DeActivate()
    {
        if (blocked || !activated)
        {
            return;
        }
        StopAllCoroutines();
        StartCoroutine(MoveDoor(transform.position, initialPosition.position));

        base.DeActivate();
    }

    public void seeIfActivate()
    {
        if (blocked)
        {
            return;
        }
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
        audios.Play();

        //base.MoveDoor(position1, position2);

        float timePassed = 0;
        float actualDistance = Vector3.Distance(position1, position2);
        float TimeToUse = (actualDistance / baseDistance) * timeToMove;

        while (timePassed / timeToMove < 1)
        {
            timePassed += Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(position1, position2, timePassed / timeToMove);
            yield return new WaitForFixedUpdate();
        }
        transform.position = position2;

        audios.Stop();
        if (levelDoor && activated)
        {
            audios.PlayOneShot(levelUp,1);
            blocked = true;
        }
        ActivateParticles(false);
    }

    void ActivateParticles(bool active)
    {
        foreach (var particle in particles)
        {
            particle.SetActive(active);
        }
    }

}

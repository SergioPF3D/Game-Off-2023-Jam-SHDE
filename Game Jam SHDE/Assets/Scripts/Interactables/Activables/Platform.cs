using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : ObjectThatMoves
{
    //no usa los puntos
    //List<Transform> pointsToMove;

    public override void Activate()
    {
        base.Activate();
        StartCoroutine(Move(transform.position, finalPosition.position));
    }

    public override void DeActivate()
    {
        base.DeActivate();
        StopAllCoroutines();
    }

    /*
    public override void AddInput(float inputs)
    {
        base.AddInput(inputs);
        SeeIfActivate();
    }

    public override void RemoveInput(float inputs)
    {
        base.RemoveInput(inputs);
        SeeIfActivate();
    }
    */

    public override IEnumerator Move(Vector3 position1, Vector3 position2)
    {      
        
        float timePassed = 0;
        while (timePassed / timeToMove < 1)
        {
            timePassed += Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(position1, position2, timePassed / timeToMove);
            yield return new WaitForFixedUpdate();
        }
        transform.position = position2;

        StartCoroutine(Move(position2, position1));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : ObjectThatMoves
{
    //no usa los puntos
    List<Transform> pointsToMove;

    public override void Start()
    {
        base.Start();
        StartCoroutine(MoveDoor(transform.position, finalPosition.position));
    }

    public override IEnumerator MoveDoor(Vector3 position1, Vector3 position2)
    {      
        
        float timePassed = 0;
        while (timePassed / timeToMove < 1)
        {
            timePassed += Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(position1, position2, timePassed / timeToMove);
            yield return new WaitForFixedUpdate();
        }
        transform.position = position2;


    }
}

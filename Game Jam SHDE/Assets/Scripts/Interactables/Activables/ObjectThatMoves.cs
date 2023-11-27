using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThatMoves : Activable
{
    public Transform initialPosition;
    public Transform finalPosition;
    
    public float timeToMove;

    public float baseDistance;
    public override void Start()
    {
        baseDistance = Vector3.Distance(initialPosition.position, finalPosition.position);
    }

    public virtual IEnumerator MoveDoor(Vector3 position1, Vector3 position2)
    {

        float timePassed = 0;
        float actualDistance = Vector3.Distance(position1, position2);
        Debug.Log(actualDistance);
        while (timePassed / timeToMove < 1)
        {
            timePassed += Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(position1, position2, timePassed / timeToMove);
            yield return new WaitForFixedUpdate();
        }
        transform.position = position2;
    }
}

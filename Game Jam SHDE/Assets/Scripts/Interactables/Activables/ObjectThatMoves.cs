using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThatMoves : Activable
{
    public Transform initialPosition;
    public Transform finalPosition;
    
    public float timeToMove;
    
    public virtual IEnumerator MoveDoor(Vector3 position1, Vector3 position2)
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

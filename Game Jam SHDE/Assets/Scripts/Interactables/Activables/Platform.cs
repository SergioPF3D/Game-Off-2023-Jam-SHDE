using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : ObjectThatMoves
{
    [SerializeField]
    float timeWaitingToMove;

    bool moving;

    [SerializeField]
    AudioSource audios;
    public override void Start()
    {
        base.Start();
        //baseDistance = Vector3.Distance(initialPosition.position, finalPosition.position);
        StartCoroutine(Move(initialPosition.position, finalPosition.position));
    }
    public override void Activate()
    {
        base.Activate();
        //StartCoroutine(Move(transform.position, finalPosition.position));
        moving = true;
    }

    public override void DeActivate()
    {
        base.DeActivate();
        //StopAllCoroutines();
        moving = false;
    }

    public override IEnumerator Move(Vector3 position1, Vector3 position2)
    {      
        float timePassed = 0;
        float time = Vector3.Distance(position1, position2) * timeToMove / Vector3.Distance(initialPosition.position, finalPosition.position);
        audios.Play();
        while (timePassed / time < 1)
        {
            if (moving)
            {
                timePassed += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(position1, position2, timePassed / time);
                audios.UnPause();
            }
            else
            {
                audios.Pause();
            }
            yield return new WaitForFixedUpdate();
        }
        transform.position = position2;
        audios.Stop();
        yield return new WaitForSeconds(timeWaitingToMove);
        
        StartCoroutine(Move(position2, position1));
    }
}

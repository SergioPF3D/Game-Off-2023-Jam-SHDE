using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : ObjectThatMoves
{
    [Header("Door")]
    //Things to activate
    
    [SerializeField]
    List<GameObject> particles;

    public bool blocked;

    [SerializeField]
    OcclusionPortal portal;

    [SerializeField]
    AudioSource audios;

    [SerializeField]
    bool levelDoor;

    [SerializeField]
    AudioClip levelUp;

    [SerializeField]
    Scene sceneToLoad;
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

        portal.open = true;
        StopAllCoroutines();
        StartCoroutine(Move(transform.position, finalPosition.position));
        base.Activate();
    }
    public override void DeActivate()
    {
        if (blocked || !activated)
        {
            return;
        }

        StopAllCoroutines();
        StartCoroutine(Move(transform.position, initialPosition.position));

        base.DeActivate();
    }

    public override void SeeIfActivate()
    {
        if (blocked)
        {
            return;
        }

        base.SeeIfActivate();
    }


    public override IEnumerator Move(Vector3 position1, Vector3 position2)
    {
        ActivateParticles(true);
        audios.Play();

        float timePassed = 0;

        float time = Vector3.Distance(position1, position2) * timeToMove / Vector3.Distance(initialPosition.position, finalPosition.position);
        while (timePassed / time < 1)
        {
            timePassed += Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(position1, position2, timePassed / time);
            yield return new WaitForFixedUpdate();
        }

        /*
        while (timePassed / timeToMove < 1)
        {
            timePassed += Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(position1, position2, timePassed / timeToMove);
            yield return new WaitForFixedUpdate();
        }
        */

        transform.position = position2;

        audios.Stop();
        if (levelDoor && activated)
        {
            audios.PlayOneShot(levelUp,1);
            blocked = true;

            SceneManager.LoadSceneAsync(sceneToLoad.buildIndex, LoadSceneMode.Additive);
        }
        if (!activated)
        {
            portal.open = false;
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

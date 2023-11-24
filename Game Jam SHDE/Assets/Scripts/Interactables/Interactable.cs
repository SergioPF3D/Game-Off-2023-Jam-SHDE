using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public List<Activable> activables;

    public bool activated;

    [Tooltip("How important the interactable in question is for activatables. If it is negative, instead of activating, deactivate")]
    public int weight;

    public int Bridgeweight;

    //Decalls
    public List<GameObject> decalls;

    [SerializeField]
    AudioSource audios;

    public virtual void Start()
    {
        foreach (var activable in activables)
        {
            //Para las placas de presion
            //activable.interactables.Add(this);
        }
    }
    public virtual void Interact()
    {
        if (activated)
        {
            Activate();
        }
        else
        {
            DeActivate();
        }

        //activated = !activated;
    }
    public virtual void Activate()
    {
        if (!activated)
        {
            if (activables.Count > 0)
            {
                foreach (var actibable in activables)
                {
                    //actibable.Activate();
                    
                    if (actibable.gameObject.GetComponent<Bridge>())
                    {
                        actibable.AddInput(Bridgeweight);
                    }
                    else
                    {
                        actibable.AddInput(weight);
                    }
                }
                activated = true;
            }
            if (decalls.Count > 0)
            {
                foreach (var decall in decalls)
                {
                    decall.SetActive(true);
                }
            }

            audios.Play();
        }
    }

    public virtual void DeActivate()
    {
        if (activated)
        {
            foreach (var actibable in activables)
            {
                if (actibable.gameObject.GetComponent<Bridge>())
                {
                    actibable.RemoveInput(Bridgeweight);
                }
                else
                {
                    actibable.RemoveInput(weight);
                }
               
            }
            if (decalls.Count > 0)
            {
                foreach (var decall in decalls)
                {
                    decall.SetActive(false);
                }
            }

            audios.Play();
        }

        activated = false;
    }
}

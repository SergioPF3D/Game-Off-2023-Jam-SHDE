using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public List<Activable> activables;

    public bool activated;

    [Tooltip("How important the interactable in question is for activatables. If it is negative, instead of activating, deactivate")]
    public int weight;

    public virtual void Start()
    {
        foreach (var activable in activables)
        {
            activable.interactables.Add(this);
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
                    actibable.AddInput(weight);
                }
                activated = true;
            }
        }
    }

    public virtual void DeActivate()
    {
        if (activated)
        {
            foreach (var actibable in activables)
            {
                //actibable.DeActivate();
                actibable.RemoveInput(weight);
            }
        }
        
        activated = false;
    }
}

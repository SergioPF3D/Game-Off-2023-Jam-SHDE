using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    List<Activable> activables;

    [SerializeField]
    bool activated;

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
                    actibable.AddInput(1);
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
                actibable.RemoveInput(1);
            }
        }
        
        activated = false;
    }
}

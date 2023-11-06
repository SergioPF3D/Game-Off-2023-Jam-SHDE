using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    List<Activable> actibables;

    [SerializeField]
    bool activated;

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
            foreach (var actibable in actibables)
            {
                //actibable.Activate();
                actibable.AddInput();
            }
            activated = true;
        }
    }

    public virtual void DeActivate()
    {
        if (activated)
        {
            foreach (var actibable in actibables)
            {
                //actibable.DeActivate();
                actibable.RemoveInput();
            }
        }
        
        activated = false;
    }
}

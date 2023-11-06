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
        foreach (var actibable in actibables)
        {
            actibable.Activate();
        }
        activated = true;
    }

    public virtual void DeActivate()
    {
        foreach (var actibable in actibables)
        {
            actibable.DeActivate();
        }
        activated = false;
    }
}

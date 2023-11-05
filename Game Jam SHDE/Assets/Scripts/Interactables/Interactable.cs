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
            foreach (var actibable in actibables)
            {
                actibable.Activate();
            }
        }
        else
        {
            foreach (var actibable in actibables)
            {
                actibable.DeActivate();
            }
        }

        activated = !activated;
    }
}

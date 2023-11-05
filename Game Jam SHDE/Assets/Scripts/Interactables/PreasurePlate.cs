using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreasurePlate : Interactable
{
    
    int objectsInTrigger;

    [SerializeField]
    float massToInteract;


    public override void Interact()
    {
        base.Interact();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (objectsInTrigger <= 0)
        {
            Interact();
        }
        objectsInTrigger++;
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        objectsInTrigger--;
        if (objectsInTrigger <= 0)
        {
            Interact();
        }
        
    }
}

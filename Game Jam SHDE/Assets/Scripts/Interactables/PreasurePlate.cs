using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreasurePlate : Interactable
{
    
    int objectsInTrigger;

    [SerializeField]
    float massToInteract;

    [SerializeField]
    float totalMass;

    [SerializeField]
    List<Rigidbody> objectsInThePlate;

    public override void Interact()
    {
        base.Interact();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.GetComponent<Rigidbody>())
        {
            objectsInThePlate.Add(other.gameObject.GetComponent<Rigidbody>());

            
            totalMass += other.gameObject.GetComponent<Rigidbody>().mass;
            if (totalMass >= massToInteract)
            {
                DeActivate();
            }
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.GetComponent<Rigidbody>())
        {
            objectsInThePlate.Remove(other.gameObject.GetComponent<Rigidbody>());

            
            totalMass -= other.gameObject.GetComponent<Rigidbody>().mass;
            if (totalMass < massToInteract)
            {
                Activate();
            }
            
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        float massInside = 0;

        foreach (var rigi in objectsInThePlate)
        {
            massInside += rigi.mass;
        }

        totalMass = massInside;

        if (totalMass >= massToInteract)
        {
            DeActivate();
            
        }
        if (totalMass < massToInteract)
        {
            Activate();
        }
        
    }
}

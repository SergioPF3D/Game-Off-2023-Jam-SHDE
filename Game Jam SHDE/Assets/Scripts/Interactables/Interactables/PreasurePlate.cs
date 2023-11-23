using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreasurePlate : Interactable
{
    //Mass
    [SerializeField]
    float massToInteract;

    [SerializeField]
    float maximunMass;

    [SerializeField]
    float totalMass;

    float massInside;

    [SerializeField]
    List<Rigidbody> objectsInThePlate;

    //PlateMovement
    Vector3 initialPosition;
    BoxCollider coll;

    public override void Start()
    {
        base.Start();

        initialPosition = transform.position;
        coll = gameObject.GetComponent<BoxCollider>();

        foreach (var decall in decalls)
        {
            decall.SetActive(false);
        }
    }
    public override void Interact()
    {
        base.Interact();
    }

    #region trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>())
        {
            objectsInThePlate.Add(other.gameObject.GetComponent<Rigidbody>());
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
                DeActivate();
                transform.position = initialPosition;
                coll.center = Vector3.zero;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Decides the mass
        massInside = 0;
        foreach (var rigi in objectsInThePlate)
        {
            massInside += rigi.mass;
        }
        totalMass = massInside;

        //Activates and set the position of the plate
        if (totalMass >= massToInteract && totalMass < maximunMass)
        {
            Activate();

            transform.position = initialPosition - (Vector3.up * 0.6f * transform.lossyScale.y);// 
            coll.center = Vector3.up * coll.size.y;
        }
        if (totalMass < massToInteract || totalMass > maximunMass)
        {
            DeActivate();

            if (massInside / massToInteract <= 1)
            {
                transform.position = initialPosition - (Vector3.up * 0.6f * (massInside / massToInteract) * transform.lossyScale.y);// 
                coll.center = Vector3.up * coll.size.y * (massInside / massToInteract);
            }
            else
            {
                transform.position = initialPosition - (Vector3.up * 0.6f * transform.lossyScale.y);//
                coll.center = Vector3.up * coll.size.y;
            }
        }
    }


    public override void Activate()
    {
        /*
        if (!activated)
        {
            if (activables.Count > 0)
            {
                foreach (var actibable in activables)
                {
                    //actibable.AddInput(weight);

                    if (actibable.gameObject.GetComponent<Bridge>())
                    {
                        actibable.AddInput(Bridgeweight);
                    }
                    else
                    {
                        actibable.AddInput(weight);
                    }
                }

                foreach (var decall in decalls)
                {
                    decall.SetActive(true);
                }
                activated = true;
            }
        }
        */
        base.Activate();
    }

    public override void DeActivate()
    {
        base.DeActivate();
        /*
        if (activated)
        {
            foreach (var actibable in activables)
            {
                actibable.RemoveInput(weight);
            }

            foreach (var decall in decalls)
            {
                decall.SetActive(false);
            }
        }

        activated = false;

        */
    }
    #endregion
}

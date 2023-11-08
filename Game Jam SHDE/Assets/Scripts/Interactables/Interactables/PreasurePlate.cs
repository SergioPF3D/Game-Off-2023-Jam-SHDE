using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreasurePlate : Interactable
{
    [SerializeField]
    float massToInteract;

    [SerializeField]
    float maximunMass;

    [SerializeField]
    float totalMass;

    float massInside;

    [SerializeField]
    List<Rigidbody> objectsInThePlate;

    //Bajar la placa
    Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
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
                gameObject.GetComponent<BoxCollider>().center = Vector3.zero;
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
            transform.position = initialPosition - Vector3.up;
            gameObject.GetComponent<BoxCollider>().center = Vector3.up;
        }
        if (totalMass < massToInteract || totalMass > maximunMass)
        {
            DeActivate();
            transform.position = initialPosition - Vector3.up * (massInside / massToInteract);
            gameObject.GetComponent<BoxCollider>().center = Vector3.up * (massInside / massToInteract);
        }
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            objectsInThePlate.Add(collision.gameObject.GetComponent<Rigidbody>());
        }
    }
    private void OnCollisionStay(Collision collision)
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
            transform.position = initialPosition - Vector3.up;
            //gameObject.GetComponent<BoxCollider>().center = Vector3.up;
        }
        if (totalMass < massToInteract || totalMass > maximunMass)
        {
            DeActivate();
            transform.position = initialPosition - Vector3.up * (massInside / massToInteract);
            //gameObject.GetComponent<BoxCollider>().center = Vector3.up * (massInside / massToInteract);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            objectsInThePlate.Remove(collision.gameObject.GetComponent<Rigidbody>());

            totalMass -= collision.gameObject.GetComponent<Rigidbody>().mass;
            if (totalMass < massToInteract)
            {
                DeActivate();
                transform.position = initialPosition;
                //gameObject.GetComponent<BoxCollider>().center = Vector3.zero;
            }
        }
    }
}

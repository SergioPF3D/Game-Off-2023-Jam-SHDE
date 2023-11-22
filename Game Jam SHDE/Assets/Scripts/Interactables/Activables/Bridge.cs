using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bridge : Activable
{
    [Header("Preasure Plate")]

    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    Vector3 rotationDirection;

    Rigidbody rigi;



    public override void Start()
    {
        rigi = this.gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //Que se adapte a la masa
        /*
        foreach (var item in collection)
        {

        }
        */







    
    }
    private void Update()
    {
        if (actualInputs > 0)
        {
            //Rota hacia un ladoS 
            rigi.MoveRotation(Quaternion.Euler(transform.rotation.eulerAngles + (rotationDirection * rotationSpeed * Time.deltaTime)));// + 
            
        }
        else if (actualInputs < 0)
        {
            //Rota hacia otro lado
            rigi.MoveRotation(Quaternion.Euler(transform.rotation.eulerAngles - (rotationDirection * rotationSpeed * Time.deltaTime)));// + 

        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithSpeed : ScalableObject
{
    [Header("ScaleWithSpeed")]
    Vector3 previousPosition;
    
    Vector3 baseScale;

    float speed;

    [SerializeField]
    [Tooltip("The scale when the object isnt moving")]
    float staticScale;

    [SerializeField]
    [Tooltip("The scale when the object is moving")]
    float movingScale;

    [SerializeField]
    [Tooltip("multiply the speed")]
    float scaleFactor;

    public override void Start()
    {
        base.Start();
        previousPosition = transform.position;
        baseScale = transform.localScale;
    }

    private void FixedUpdate()
    {       
        speed = Vector3.Distance(previousPosition, transform.position);
        //speed = (this.gameObject.GetComponent<Rigidbody>().velocity.x + this.gameObject.GetComponent<Rigidbody>().velocity.y + this.gameObject.GetComponent<Rigidbody>().velocity.z) / 3;

        previousPosition = transform.position;

        speed *= scaleFactor;

        if (speed < staticScale)
        {
            speed = staticScale;
        }
        else if (speed > movingScale)
        {
            speed = movingScale;
        }
        
        transform.localScale = baseScale * speed;
    }
}

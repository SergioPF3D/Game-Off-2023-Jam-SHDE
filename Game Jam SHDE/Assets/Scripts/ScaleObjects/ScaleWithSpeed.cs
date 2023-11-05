using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithSpeed : MonoBehaviour
{
    
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
    [Tooltip("The target that we moves")]
    float scaleFactor;

    private void Start()
    {
        previousPosition = transform.position;
        baseScale = transform.localScale;
    }

    private void FixedUpdate()
    {       
        speed = Vector3.Distance(previousPosition, transform.position);
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

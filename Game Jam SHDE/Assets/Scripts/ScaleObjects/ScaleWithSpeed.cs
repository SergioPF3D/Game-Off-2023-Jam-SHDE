using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithSpeed : MonoBehaviour
{
    
    Vector3 previousPosition;
    
    Vector3 baseScale;

    float speed;

    [SerializeField]
    float minScale;

    [SerializeField]
    float maxScale;

    [SerializeField]
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

        if (speed < minScale)
        {
            speed = minScale;
        }
        else if (speed > maxScale)
        {
            speed = maxScale;
        }
        
        transform.localScale = baseScale * speed;
    }
}

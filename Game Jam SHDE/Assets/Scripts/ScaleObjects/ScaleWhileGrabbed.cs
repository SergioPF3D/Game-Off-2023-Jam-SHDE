using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWhileGrabbed : MonoBehaviour
{
    Vector3 baseScale;
    [SerializeField]
    Vector3 maxScale;

    [SerializeField]
    float maxTime;

    [SerializeField]
    float actualTime;

    float interpolation;
    
    bool scalating;
    bool deScalating;

    private void Start()
    {
        baseScale = transform.localScale;
    }

    private void Update()
    {
        if (scalating)
        {
            if (actualTime < maxTime)
            {
                actualTime += Time.deltaTime;
            }
            
            interpolation  = actualTime / maxTime;
            Scalate();

            if (interpolation >= 1)
            {
                scalating = false;
            }
        }
        else if (deScalating)
        {
            if (actualTime > 0)
            {
                actualTime -= Time.deltaTime;
            }

            interpolation = actualTime / maxTime;
            Scalate();

            if (interpolation <= 0)
            {
                deScalating = false;
            }
        }
    }

    private void OnMouseDown()
    {
        scalating = true;
        deScalating = false;
    }
    
    private void OnMouseUp()
    {
        scalating = false;
        deScalating = true;
    }

    void Scalate()
    {
        transform.localScale = Vector3.Lerp(baseScale, maxScale, interpolation);
    }
}

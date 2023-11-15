using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalateWithDistance : ScalableObject
{
    [Header("ScalateWithDistance")]

    [SerializeField]
    [Tooltip("what is the object from which it detects distance")]
    Transform ObjectDistant;

    [SerializeField]
    [Tooltip("The distance from which, if the ObjectDistant approaches, the scale no longer changes. Must be non negative")]
    float minDistance;

    [SerializeField]
    [Tooltip("the scale when the object is at minDistance or less")]
    Vector3 minScale;

    [SerializeField]
    [Tooltip("the distance from which, if the ObjectDistant moves away, the scale does not change any more. Must be greater than minDistance")]
    float maxDistance;

    [SerializeField]
    [Tooltip("the scale when the object is at maxDistance or more")]
    Vector3 maxScale;
    
    void Update()
    {
        if (ObjectDistant)
        {
            if (Vector3.Distance(this.transform.position, ObjectDistant.transform.position) <= minDistance)
            {
                this.transform.localScale = minScale;
            }
            else if (Vector3.Distance(this.transform.position, ObjectDistant.transform.position) >= maxDistance)
            {
                this.transform.localScale = maxScale;
            }
            else
            {
                this.transform.localScale = Vector3.Lerp(minScale, maxScale, (Vector3.Distance(this.transform.position, ObjectDistant.transform.position) - minDistance) / (maxDistance - minDistance));
            }
        }

        ChangeMass();
    }
}

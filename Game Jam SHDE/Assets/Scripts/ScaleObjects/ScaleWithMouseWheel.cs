using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithMouseWheel : ScalableObject
{
    [Header("ScaleWithMouseWheel")]

    [SerializeField]
    [Tooltip("The speed at which the object scales")]
    Vector3 scaleSpeed;

    [SerializeField]
    [Tooltip("The minimum scale you can aply to the object")]
    float minScale;

    [SerializeField]
    [Tooltip("The maximun scale you can aply to the object")]
    float maxScale;

    public void Scalate()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //Scale up
            if (transform.localScale.x + Input.GetAxis("Mouse ScrollWheel") * scaleSpeed.x <= maxScale && transform.localScale.y + Input.GetAxis("Mouse ScrollWheel") * scaleSpeed.y <= maxScale && transform.localScale.z + Input.GetAxis("Mouse ScrollWheel") * scaleSpeed.z <= maxScale)
            {
                transform.localScale += Input.GetAxis("Mouse ScrollWheel") * scaleSpeed;
                ChangeMass();
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //Scale down
            if (transform.localScale.x + Input.GetAxis("Mouse ScrollWheel") * scaleSpeed.x >= minScale && transform.localScale.y + Input.GetAxis("Mouse ScrollWheel") * scaleSpeed.y >= minScale && transform.localScale.z + Input.GetAxis("Mouse ScrollWheel") * scaleSpeed.z >= minScale)
            {
                transform.localScale += Input.GetAxis("Mouse ScrollWheel") * scaleSpeed;
                ChangeMass();
            }
        }  
    }
}

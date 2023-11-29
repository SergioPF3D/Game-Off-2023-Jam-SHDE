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

    public void Scalate(float mousewheelScalate)
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //Scale up
            if (transform.localScale.x + Input.GetAxis("Mouse ScrollWheel") * scaleSpeed.x * mousewheelScalate  <= maxScale && transform.localScale.y + Input.GetAxis("Mouse ScrollWheel") * scaleSpeed.y * mousewheelScalate  <= maxScale && transform.localScale.z + Input.GetAxis("Mouse ScrollWheel") * scaleSpeed.z * mousewheelScalate  <= maxScale)
            {
                transform.localScale += Input.GetAxis("Mouse ScrollWheel") * scaleSpeed * mousewheelScalate;
                ChangeMass();
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //Scale down
            if (transform.localScale.x + Input.GetAxis("Mouse ScrollWheel") * scaleSpeed.x * mousewheelScalate  >= minScale && transform.localScale.y + Input.GetAxis("Mouse ScrollWheel") * scaleSpeed.y * mousewheelScalate  >= minScale && transform.localScale.z + Input.GetAxis("Mouse ScrollWheel") * scaleSpeed.z * mousewheelScalate  >= minScale)
            {
                transform.localScale += Input.GetAxis("Mouse ScrollWheel") * scaleSpeed * mousewheelScalate;
                ChangeMass();
            }
        }  
    }
}

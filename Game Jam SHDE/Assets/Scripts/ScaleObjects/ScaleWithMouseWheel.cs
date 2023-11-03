using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithMouseWheel : MonoBehaviour
{
    [SerializeField]
    Vector3 scaleSpeed;

    [SerializeField]
    float minScale;

    [SerializeField]
    float maxScale;

    private void OnMouseOver()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //Agranda
            if (transform.localScale.x + Input.GetAxis("Mouse ScrollWheel") <= maxScale || transform.localScale.y + Input.GetAxis("Mouse ScrollWheel") <= maxScale || transform.localScale.z + Input.GetAxis("Mouse ScrollWheel") <= maxScale)
            {
                transform.localScale += Input.GetAxis("Mouse ScrollWheel") * scaleSpeed;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //Empequeñece
            if (transform.localScale.x + Input.GetAxis("Mouse ScrollWheel") >= minScale || transform.localScale.y + Input.GetAxis("Mouse ScrollWheel") >= minScale || transform.localScale.z + Input.GetAxis("Mouse ScrollWheel") >= minScale)
            {
                transform.localScale += Input.GetAxis("Mouse ScrollWheel") * scaleSpeed;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ScaleWithMouseWheel : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The speed at which the object scales")]
    Vector3 scaleSpeed;

    [SerializeField]
    [Tooltip("The minimum scale you can aply to the object")]
    float minScale;

    [SerializeField]
    [Tooltip("The maximun scale you can aply to the object")]
    float maxScale;

    private void OnMouseOver()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //Scale up
            if (transform.localScale.x + Input.GetAxis("Mouse ScrollWheel") <= maxScale || transform.localScale.y + Input.GetAxis("Mouse ScrollWheel") <= maxScale || transform.localScale.z + Input.GetAxis("Mouse ScrollWheel") <= maxScale)
            {
                transform.localScale += Input.GetAxis("Mouse ScrollWheel") * scaleSpeed;
                this.GetComponent<Rigidbody>().mass = (this.transform.localScale.x + this.transform.localScale.y + this.transform.localScale.z) / 3;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //Scale down
            if (transform.localScale.x + Input.GetAxis("Mouse ScrollWheel") >= minScale || transform.localScale.y + Input.GetAxis("Mouse ScrollWheel") >= minScale || transform.localScale.z + Input.GetAxis("Mouse ScrollWheel") >= minScale)
            {
                transform.localScale += Input.GetAxis("Mouse ScrollWheel") * scaleSpeed;
                this.GetComponent<Rigidbody>().mass = (this.transform.localScale.x + this.transform.localScale.y + this.transform.localScale.z) / 3;
            }
        }  
    }
}

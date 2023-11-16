using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ScalableObject : MonoBehaviour
{
    [Header("ScalableObject")]

    public Color emisiveColor;
    public float baseEmisiveIntensity;

    //Return to initial position
    Vector3 basePosition;

    public virtual void Start()
    {
        basePosition = transform.position;

        this.gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmisiveColor",emisiveColor);
        baseEmisiveIntensity = this.gameObject.GetComponent<MeshRenderer>().material.GetFloat("_EmisiveIntensity");

        ChangeMass();
    }
    public virtual void ChangeMass()
    {
        this.GetComponent<Rigidbody>().mass = (this.transform.localScale.x + this.transform.localScale.y + this.transform.localScale.z) / 3;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            transform.position = basePosition;
            //Habria que recoger la rotacion inicial
            //transform.rotation = Quaternion.Euler(Vector3.zero);
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}

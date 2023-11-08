using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ScaleWhileGrabbed : ScalableObject
{
    [Header("ScaleWhileGrabbed")]

    Vector3 baseScale;

    [SerializeField]
    [Tooltip("The scale that the object reachs while grabbed")]
    Vector3 grabbedScale;

    [SerializeField]
    [Tooltip("The time that the object late scalating")]
    float timeToScale;

    float actualTime; //The time that the objetc has been scalating
    float interpolation; //The percentaje of the time that the object has been scalating
    
    bool scalating; //If the object is scalating
    bool deScalating; //If the object is de-scalating

    MoveObjects player;

    public override void Start()
    {
        base.Start();
        baseScale = transform.localScale;
        player = GameObject.FindObjectOfType<MoveObjects>();
    }

    private void Update()
    {
        //Is is scalating, calculate the percentaje of time and interpolate between de grabbed scale an its base scale
        if (scalating)
        {
            if (player.target == this.transform)
            {
                if (actualTime < timeToScale)
                {
                    actualTime += Time.deltaTime;
                }

                interpolation = actualTime / timeToScale;
                Scalate();

                if (interpolation >= 1)
                {
                    scalating = false;
                }

                this.GetComponent<Rigidbody>().mass = (this.transform.localScale.x + this.transform.localScale.y + this.transform.localScale.z) / 3;
            }
            
        }
        //scalating inverted
        else if (deScalating)
        {
            if (actualTime > 0)
            {
                actualTime -= Time.deltaTime;
            }

            interpolation = actualTime / timeToScale;
            Scalate();

            if (interpolation <= 0)
            {
                deScalating = false;
            }

            this.GetComponent<Rigidbody>().mass = (this.transform.localScale.x + this.transform.localScale.y + this.transform.localScale.z) / 3;
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
        transform.localScale = Vector3.Lerp(baseScale, grabbedScale, interpolation);
    }
}

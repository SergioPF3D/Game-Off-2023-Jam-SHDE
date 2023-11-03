using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalateWithDistance : MonoBehaviour
{
    [SerializeField]
    float baseDistance;

    [SerializeField]
    Transform ObjectDistant;

    Vector3 baseScale;

    [SerializeField]
    float scaleFactor;
    //Podriamos separar los ejes, con un vector3, velocidad d eescalado
    //Que pueda ser inversamente proporcional

    [SerializeField]
    float minScale;

    [SerializeField]
    float maxScale;

    private void Start()
    {
        baseScale = this.transform.localScale;
    }
    void Update()
    {
        if (ObjectDistant)
        {
            if (Vector3.Distance(this.transform.position, ObjectDistant.transform.position) / baseDistance * scaleFactor < minScale)
            {
                this.transform.localScale = baseScale * minScale;
            }
            else if (Vector3.Distance(this.transform.position, ObjectDistant.transform.position) / baseDistance * scaleFactor > maxScale)
            {
                this.transform.localScale = baseScale * maxScale;
            }else
            {
                this.transform.localScale = baseScale * Vector3.Distance(this.transform.position, ObjectDistant.transform.position) / baseDistance * scaleFactor;
            }


            //Donde meto ScaleFactor para que al estar dentro de basedistance se haga pequeño mas rapido y fuera se haga grande mas rapido
            //Ademas poder hacerlo al reves
            //this.transform.localScale = baseScale * Vector3.Distance(this.transform.position, ObjectDistant.transform.position) / baseDistance;
        }
    }
}

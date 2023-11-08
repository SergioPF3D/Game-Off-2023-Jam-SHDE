using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableObject : MonoBehaviour
{
    [Header("ScalableObject")]

    public Color emisiveColor;
    public float baseEmisiveIntensity;
    public virtual void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmisiveColor",emisiveColor);
        baseEmisiveIntensity = this.gameObject.GetComponent<MeshRenderer>().material.GetFloat("_EmisiveIntensity");
    }
}

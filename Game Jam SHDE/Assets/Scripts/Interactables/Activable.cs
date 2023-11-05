using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activable : MonoBehaviour
{
    BoxCollider colider;
    MeshRenderer render;
    private void Start()
    {
        colider = this.gameObject.GetComponent<BoxCollider>();
        render = this.gameObject.GetComponent<MeshRenderer>();
    }

    public void Activate()
    {
        colider.enabled = true;
        render.enabled = true;
    }
    public void DeActivate()
    {
        colider.enabled = false;
        render.enabled = false;
    }
}

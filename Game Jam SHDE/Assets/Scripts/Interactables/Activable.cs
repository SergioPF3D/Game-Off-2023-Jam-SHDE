using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activable : MonoBehaviour
{
    [SerializeField]
    bool baseActivated;

    //Condition
    [SerializeField]
    float inputs;
    [SerializeField]
    float actualInputs;

    //Things to activate
    BoxCollider colider;
    MeshRenderer render;
    private void Start()
    {
        colider = this.gameObject.GetComponent<BoxCollider>();
        render = this.gameObject.GetComponent<MeshRenderer>();

        if (!baseActivated)
        {
            DeActivate();
        }
    }

    void Activate()
    {
        colider.enabled = true;
        render.enabled = true;
    }
    void DeActivate()
    {
        colider.enabled = false;
        render.enabled = false;
    }

    void seeIfActivate()
    {
        if (actualInputs >= inputs)
        {
            if (baseActivated)
            {
                DeActivate();
            }
            else
            {
                Activate();
            }
        }
        else
        {
            if (baseActivated)
            {
                Activate();
            }
            else
            {
                DeActivate();
            }
        }
    }
    public void AddInput()
    {
        actualInputs++;
        seeIfActivate();
    }
    public void RemoveInput()
    {
        actualInputs--;
        seeIfActivate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activable : MonoBehaviour
{
    [Header("Activable")]

    [SerializeField]
    public bool baseActivated;
    public bool activated;

    //Condition
    [SerializeField]
    public float inputs;

    [SerializeField]
    public float actualInputs;

    public List<Interactable> interactables;

    
    public virtual void Start()
    {
        if (baseActivated)
        {
            Activate();
            activated = true;
        }
    }

    public virtual void Activate()
    {
        activated = true;
    }
    public virtual void DeActivate()
    {
        activated = false;
    }

   
    public virtual void AddInput(float inputs)
    {
        actualInputs += inputs;
    }
    public virtual void RemoveInput(float inputs)
    {
        actualInputs -= inputs;
    }
}

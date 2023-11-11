using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activable : MonoBehaviour
{
    [Header("Activable")]

    [SerializeField]
    public bool baseActivated;

    //Condition
    [SerializeField]
    public float inputs;

    [SerializeField]
    public float actualInputs;

    public List<Interactable> interactables;

    
    public virtual void Start()
    {
        if (!baseActivated)
        {
            DeActivate();
        }
    }

    public virtual void Activate()
    {

    }
    public virtual void DeActivate()
    {

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

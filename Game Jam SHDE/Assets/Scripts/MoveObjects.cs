using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MoveObjects : MonoBehaviour
{
	[Header("Components")]

	[SerializeField]
	[Tooltip("The target that we moves")]
	Transform target;

	[Header("Base Parameters")]

	[SerializeField]
	[Tooltip("Layermask to detect the Movable and Scalable objects")]
	LayerMask targetMask;
	[SerializeField]
	[Tooltip("Layermask that excludes the Movable and Scalable objects to detect just the envyroment and move them")]
	LayerMask ignoreTargetMask;

	[SerializeField]
	[Tooltip("The empty object that places where we need to place the target to direct it")]
	float maxDistance;

	
	[Header("Movement")]

	[SerializeField]
	[Tooltip("The empty object that places where we need to place the target to direct it")]
	Transform grabber;

	[SerializeField]
	[Tooltip("Multiplied by the sensitivity and the distance between the target and the objective, it decides the speed at which the target goes towards the desired point")]
	float chasingSpeed;

	[SerializeField]
	[Tooltip("The reduction factor when the object is thrown")]
	float speedReductionWhenThrown;
	
	//Mouse
	float mouseSensibility;

	//[Header("Scale")]
	float baseDistance;
	Vector3 baseScale;           
	float currentDistance;

	[Header("Ray VFX")]

	[SerializeField]
	[Tooltip("The ray VFX gameobject")]
	VisualEffect rayVFX;

	[SerializeField]
	[Tooltip("Final point of the ray")]
	Transform rayTarget;
	
	void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		//Esto habra que cambiarlo con un m�todo cuando se cambie la sensibilidad
		mouseSensibility = GameObject.FindObjectOfType<Player>().mouseSensibility;
	}

	void Update()
	{
		DetectInputs();

		MoveTarget();
	}

	void DetectInputs()
	{

		if (Input.GetMouseButtonDown(0))
		{
			//If we dont have a target, we release it;
			if (target == null)
			{
				RaycastHit hit;
				if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, targetMask))
				{
					//If we are so far of the target, then we dont set it
                    if (Vector3.Distance(hit.transform.position, transform.position) > maxDistance)
                    {
						return;
                    }

					target = hit.transform;
					
					baseDistance = Vector3.Distance(transform.position, target.position);
					baseScale = target.localScale;

					rayVFX.gameObject.SetActive(true);
					rayVFX.SetMesh("RendererMeshParticle", target.gameObject.GetComponent<MeshFilter>().mesh);
				}
			}
		}
        if (Input.GetMouseButtonUp(0))
        {
			//If we have a target, we have to release it
			if (target)
            {
				//Al habr� que hacer con esto para que no pueda haber excesos
				//target.GetComponent<Rigidbody>().velocity /= speedReductionWhenThrown * Vector3.Distance(grabber.position, target.transform.position);

				rayVFX.gameObject.SetActive(false);
				target = null;
			}
		}
	}

	void MoveTarget()
	{
		//If theres no target, we return
		if (!target)
		{
			return;
		}

		//If we have someting in front, we move the object towards it and scale if necesary, if not, move the object with distance
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ignoreTargetMask))
		{
			//We move the target
			grabber.position = hit.point;

			currentDistance = Vector3.Distance(transform.position, target.position);
			
			//If its scalable, we scale it
			if (target.gameObject.layer == 6)
			{
				// Calculate the ratio between the current distance and the original distance
				float ratio = currentDistance / baseDistance;
				
				//Scale the object
				target.localScale = ratio * baseScale;
			}
		}
		else
		{
			grabber.position = transform.position + transform.forward * currentDistance;
		}

		//Set the speed of the target
		target.GetComponent<Rigidbody>().velocity = (grabber.position - target.transform.position).normalized * Vector3.Distance(grabber.position, target.transform.position) * mouseSensibility * chasingSpeed;
		
		rayTarget.position = target.position;
		rayTarget.localScale = target.localScale;
	}
}

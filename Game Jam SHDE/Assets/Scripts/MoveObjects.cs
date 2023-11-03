using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
	[Header("Components")]
	public Transform target;            // The target object we picked up for scaling

	[Header("Parameters")]
	[SerializeField]
	LayerMask targetMask;
	[SerializeField]			// The layer mask used to hit only potential targets with a raycast
	LayerMask ignoreTargetMask;  // The layer mask used to ignore the player and target objects while raycasting
	[SerializeField]
	float offsetFactor;
	[SerializeField]            // The offset amount for positioning the object so it doesn't clip into walls
	float maxDistance;

	float originalDistance;             // The original distance between the player camera and the target
	float originalScale;                // The original scale of the target objects prior to being resized
	Vector3 targetScale;                // The scale we want our object to be set to each frame

	float currentDistance;


	//Ray
	[SerializeField]
	Transform vfxObjective;

	[SerializeField]
	GameObject vfxRay;
	


	float chasingSpeed;
	void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		HandleInput();

		MoveTarget();
	}

	void HandleInput()
	{
		// Check for left mouse click
		if (Input.GetMouseButtonDown(0))
		{
			// If we do not currently have a target
			if (target == null)
			{
				// Fire a raycast with the layer mask that only hits potential targets
				RaycastHit hit;
				if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, targetMask))
				{
                    if (Vector3.Distance(hit.transform.position, transform.position) > maxDistance)
                    {
						return;
                    }

					// Set our target variable to be the Transform object we hit with our raycast
					target = hit.transform;
					vfxRay.SetActive(true);

					// Disable physics for the object
					target.GetComponent<Rigidbody>().isKinematic = true;

					// Calculate the distance between the camera and the object
					originalDistance = Vector3.Distance(transform.position, target.position);

					// Save the original scale of the object into our originalScale Vector3 variabble
					originalScale = target.localScale.x;

					// Set our target scale to be the same as the original for the time being
					targetScale = target.localScale;
				}
			}
		}
        if (Input.GetMouseButtonUp(0))
        {
            if (target)
            {
				// Reactivate physics for the target object
				target.GetComponent<Rigidbody>().isKinematic = false;

				// Set our target variable to null
				target = null;

				vfxRay.SetActive(false);
			}


			
		}

	}

	void MoveTarget()
	{
		if (!target)
		{
			return;
		}
		
		

		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ignoreTargetMask))
		{
			//Make a media of the scales of the object, to aproximate the offset that the object may have
			float scaleMedia = (target.localScale.x + target.localScale.y + target.localScale.z) / 3;//(targetScale.x + targetScale.y + targetScale.z) / 3;

			currentDistance = Vector3.Distance(transform.position, target.position);
			
			if (target.gameObject.layer == 6)
			{
				//target.position = hit.point - transform.forward * offsetFactor * scaleMedia;
				target.position = hit.point + (hit.normal.normalized * scaleMedia / 2);

				// Calculate the ratio between the current distance and the original distance
				float s = currentDistance / originalDistance;
				// Set the scale Vector3 variable to be the ratio of the distances
				targetScale.x = targetScale.y = targetScale.z = s;


				// Set the scale for the target objectm, multiplied by the original scale
				target.localScale = targetScale * originalScale;
			}
			else
			{
				
                if (Vector3.Distance(transform.position, hit.point + (hit.normal.normalized * scaleMedia / 2)) < Vector3.Distance(transform.position, transform.position + transform.forward * currentDistance))
                {
					target.position = hit.point + (hit.normal.normalized * scaleMedia / 2);
                }
                else
                {
					target.transform.position = transform.position + transform.forward * currentDistance;
				}
				target.position = hit.point + (hit.normal.normalized * scaleMedia / 2);
			}
		}
		else
		{
			//target.position = transform.posi
			target.transform.position = transform.position + transform.forward * currentDistance;

		}

		vfxObjective.position = target.transform.position;


	}

	void TargetChasing()
    {
		Transform target = this.transform;
		Transform chasedPoint = this.transform;

		target.GetComponent<Rigidbody>().velocity = (chasedPoint.position - target.position).normalized * Vector3.Distance(chasedPoint.position, target.position) * chasingSpeed;

	}
}

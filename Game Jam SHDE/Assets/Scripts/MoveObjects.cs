using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Camera))]
public class MoveObjects : MonoBehaviour
{
	[Header("Components")]
	
	[Tooltip("The target that we moves")]
	public Transform target;

	[Header("Base Parameters")]

	[SerializeField]
	[Tooltip("Layermask to detect the Movable and Scalable objects")]
	LayerMask targetMask;
	[SerializeField]
	[Tooltip("Layermask that excludes the Movable and Scalable objects to detect just the envyroment and move them")]
	LayerMask ignoreTargetMask;

	[SerializeField]
	[Tooltip("")]
	float maxDistance;

	[SerializeField]
	[Tooltip("")]
	float minDistance;

	//Movement
	[Header("Movement")]

	[SerializeField]
	[Tooltip("The empty object that places where we need to place the target to direct it")]
	Transform grabber;


		//Scrollwheel
	[SerializeField]
	float distance;
	[SerializeField]
	bool moveOrInteract;
	[SerializeField]
	float scrollWheel;


	[SerializeField]
	[Tooltip("Multiplied by the sensitivity and the distance between the target and the objective, it decides the speed at which the target goes towards the desired point")]
	float chasingSpeed;

	[SerializeField]
	[Tooltip("limit the moving speed, if its 0 or negative its limitless, also its funnier")]
	float maxSpeed;

	[SerializeField]
	[Tooltip("nearer than that distance, When you let go of the cube, it will have no speed.")]
	float distanceToThrow;

	[SerializeField]
	bool fly;

	//Mouse
	float mouseSensibility;

	[Header("Scale")]
	float baseDistance;
	Vector3 baseScale;
	float baseMass;

	float currentDistance;

	[Header("Aesthetics")]

	[SerializeField]
	[Tooltip("The ray VFX gameobject")]
	VisualEffect rayVFX;

	[SerializeField]
	[Tooltip("Final point of the ray")]
	Transform rayTarget;

	[SerializeField]
	Material sphereMaterial;

	[SerializeField]
	float shaderEmisiveIntensity;

	[SerializeField]
	float rayEmisiveInetnsity;

	[SerializeField]
	Color moveColor;

	[SerializeField]
	Color scalateColor;

	//_FresnelColor
	/*
	[SerializeField]
	float intensidad;

	[SerializeField]
	Material materi;
	*/

	[Header("Animation")]
	[SerializeField]
	Animator staffAnimationController;

	void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		//Esto habra que cambiarlo con un método cuando se cambie la sensibilidad
		mouseSensibility = GameObject.FindObjectOfType<Player>().mouseSensibility;

		//staffAnimationController = GameObject.FindObjectOfType<Player>().staffAnimationController;

		//emisiveColor = sphereMaterial.GetColor("_FresnelColor");
		ChangeColor();

		sphereMaterial.SetColor("_FresnelColor", Color.black);
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
					target.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
					distance = baseDistance = Vector3.Distance(transform.position, target.position);
					baseScale = target.localScale;
					baseMass = target.GetComponent<Rigidbody>().mass;
					target.GetComponent<Rigidbody>().isKinematic = false;

					rayVFX.gameObject.SetActive(true);
					rayVFX.SetMesh("RendererMeshParticle", target.gameObject.GetComponent<MeshFilter>().mesh);
					target.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_EmisiveIntensity", shaderEmisiveIntensity);

					staffAnimationController.SetBool("Grabbing", true);
				}
			}
		}
        if (Input.GetMouseButtonUp(0))
        {

			Debug.Log(1);
			//If we have a target, we have to release it
			if (target)
            {
                if (Vector3.Distance(target.position, grabber.position) < distanceToThrow)
                {
					//Make Speed 0
					target.GetComponent<Rigidbody>().velocity = Vector3.zero;

				}
                else
                {
					//Limit the speed
					float media = (Mathf.Abs(target.GetComponent<Rigidbody>().velocity.x) + Mathf.Abs(target.GetComponent<Rigidbody>().velocity.y) + Mathf.Abs(target.GetComponent<Rigidbody>().velocity.z)) / 3;
					if (maxSpeed > 0)
					{
						if (media > maxSpeed)
						{
							target.GetComponent<Rigidbody>().velocity *= (maxSpeed / media);
						}
					}
				}
				
				target.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

				rayVFX.gameObject.SetActive(false);
				sphereMaterial.SetColor("_FresnelColor", Color.black);
				target.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_EmisiveIntensity", target.gameObject.GetComponent<ScalableObject>().baseEmisiveIntensity);

				staffAnimationController.SetBool("Grabbing", false);

				target = null;
			}
		}

        if (Input.GetKeyDown(KeyCode.E))
        {
			moveOrInteract = !moveOrInteract;
			ChangeColor();
		}
	}

	void MoveTarget()
	{
		//If theres no target, we return
		if (!target)
		{
			return;
		}

        if (!moveOrInteract)
        {
			//Interact
			if (target.GetComponent<ScaleWithMouseWheel>())
			{
				target.GetComponent<ScaleWithMouseWheel>().Scalate();
			}
        }
        else
        {
			distance = Mathf.Clamp(distance + Input.GetAxis("Mouse ScrollWheel") * mouseSensibility, minDistance, maxDistance);
		}
		
		//Set the grabber
		if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance, ignoreTargetMask))
		{
			grabber.position = hit.point;
        }
        else
        {
			if (moveOrInteract)
			{
				grabber.position = transform.position + transform.forward * distance;
			}
		}

		//If its scalable, we scale it
		if (target.gameObject.layer == 6)
		{
			currentDistance = Vector3.Distance(transform.position, target.position);
			//Calculate the ratio between the current distance and the original distance
			//Scale the object
			target.localScale = currentDistance / baseDistance * baseScale;
			target.GetComponent<Rigidbody>().mass = currentDistance / baseDistance * baseMass;
		}

		rayTarget.position = target.position;
		rayTarget.localScale = target.localScale;

		//We move the target
		if (!fly)
        {
            if (Physics.Raycast(target.position, grabber.position - target.position, out RaycastHit hitted, distance))
            {
                if (hitted.collider.gameObject.layer == 3)
                {
					return;
				}
            }
		}

		target.GetComponent<Rigidbody>().velocity = (grabber.position - target.transform.position).normalized * Vector3.Distance(grabber.position, target.transform.position) * mouseSensibility * chasingSpeed;
	}

	void ChangeColor()
    {
		if (moveOrInteract)
		{
			rayVFX.SetVector4("Color", moveColor * rayEmisiveInetnsity);
			sphereMaterial.SetColor("_FresnelColor", moveColor * shaderEmisiveIntensity);
		}
		else
		{
			rayVFX.SetVector4("Color", scalateColor * rayEmisiveInetnsity);
			sphereMaterial.SetColor("_FresnelColor", scalateColor * shaderEmisiveIntensity);
		}
	}
}

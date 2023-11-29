using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

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
	LayerMask blockcubes;

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

	[Space(20)]

	[SerializeField]
	bool fly;

	[SerializeField]
	float angle;

	[SerializeField]
	float distancetoFly;

	[Header("Scale")]
	float baseDistance;
	Vector3 baseScale;
	float baseMass;

	float currentDistance;

	[Header("Aesthetics")]

	//VFX
	[SerializeField]
	[Tooltip("The ray VFX gameobject")]
	VisualEffect rayVFX;

	[SerializeField]
	[Tooltip("The ray VFX gameobject")]
	VisualEffect sphereVFX;

	[SerializeField]
	[Tooltip("Final point of the ray")]
	Transform rayTarget;

	[Space(10)] //Spherematerial
	[SerializeField]
	Material sphereMaterial;

	[SerializeField]
	float shaderEmisiveIntensity;

	[Space(10)] //Outline
	[SerializeField]
	Material outline;

	[SerializeField]
	public GameObject outlined;

	[SerializeField]
	float outlineWidth;

	[Space(10)]//Light

	[SerializeField]
	Light sphereLight;

	[Space(10)]//Decall

	[SerializeField]
	GameObject cubeShadow;

	[Header("Animation")]
	[SerializeField]
	Animator staffAnimationController;

	[Header("UI")]
	[SerializeField]
	Slider slid;

	void Start()
	{
		//Esto habra que cambiarlo con un método cuando se cambie la sensibilidad
		//mouseSensibility = GameObject.FindObjectOfType<Player>().mouseSensibility;
		
		//staffAnimationController = GameObject.FindObjectOfType<Player>().staffAnimationController;

		//emisiveColor = sphereMaterial.GetColor("_FresnelColor");
		ChangeColor();
	}

	void Update()
	{
		DetectInputs();

		
	}
    private void FixedUpdate()
    {
		MoveTarget(); 
	}

    void DetectInputs()
	{
		if (Input.GetMouseButtonDown(0))
		{
			//If we dont have a target, we release it;
			if (target == null)
			{
				RaycastHit cubeDetected;
				if (Physics.Raycast(transform.position, transform.forward, out cubeDetected, Mathf.Infinity, targetMask))
				{
					//If we are so far of the target, then we dont set it
					if (Vector3.Distance(cubeDetected.point, transform.position) > maxDistance)
					{
						return;
                    }
					//Si hay algo en medio
					if (Physics.Raycast(transform.position, transform.forward, out RaycastHit obstacle, Vector3.Distance(transform.position, cubeDetected.point), ignoreTargetMask))
					{
						return;
					}

					target = cubeDetected.transform;
					target.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
					distance = baseDistance = Vector3.Distance(transform.position, target.position);
					baseScale = target.localScale;
					baseMass = target.GetComponent<Rigidbody>().mass;
					target.GetComponent<Rigidbody>().isKinematic = false;

					rayVFX.gameObject.SetActive(true);
					rayVFX.SetMesh("RendererMeshParticle", target.gameObject.GetComponent<MeshFilter>().mesh);
					sphereVFX.SetBool("Bool_ParticleActivated", true);
					target.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_EmisiveIntensity", shaderEmisiveIntensity);

					staffAnimationController.SetBool("Grabbing", true);

					cubeShadow.SetActive(true);
					//cubeShadow.transform.position = target.transform.position;
					//cubeShadow.transform.rotation = Quaternion.Euler(new Vector3(90,0,0));
					//cubeShadow.transform.parent = target.transform;

					sphereLight.intensity += 0.01f;

					if (target.GetComponent<ScaleWhileGrabbed>())
					{
						target.GetComponent<ScaleWhileGrabbed>().ChangeMode(true);
					}
				}

			}
		}
        if (Input.GetMouseButtonUp(0))
        {
			//If we have a target, we have to release it
			if (target)
            {
				if (target.GetComponent<ScaleWhileGrabbed>())
				{
					target.GetComponent<ScaleWhileGrabbed>().ChangeMode(false);
				}

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
				sphereVFX.SetBool("Bool_ParticleActivated", false);
				target.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_EmisiveIntensity", target.gameObject.GetComponent<ScalableObject>().baseEmisiveIntensity);

				staffAnimationController.SetBool("Grabbing", false);

				cubeShadow.SetActive(false);

				sphereLight.intensity -= 0.01f;

				target = null;
			}
		}

        if (Input.GetKeyDown(KeyCode.E))
        {
			moveOrInteract = !moveOrInteract;
			ChangeColor();
		}

		//Set outline
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit cubeToOutline, maxDistance, targetMask))
        {
			//Si hay algo en medio
			if (Physics.Raycast(transform.position, transform.forward, out RaycastHit obstacle, Vector3.Distance(transform.position, cubeToOutline.point), ignoreTargetMask))
			{
				return;
			}

			if (!target)
            {
				if (outlined && outlined != cubeToOutline.collider.gameObject)
				{

					outlined.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 0.1f);
					outlined.gameObject.layer = outlined.gameObject.GetComponent<ScalableObject>().layer;

					cubeToOutline.collider.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", outlineWidth);
					cubeToOutline.collider.gameObject.layer = 12;
					outlined = cubeToOutline.collider.gameObject;
				}

				if (!outlined)
				{
					cubeToOutline.collider.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", outlineWidth);
					cubeToOutline.collider.gameObject.layer = 12;
					outlined = cubeToOutline.collider.gameObject;
				}
			}
		}
        else
        {
            if (!target && outlined)
            {
				outlined.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 0.1f);
				outlined.gameObject.layer = outlined.gameObject.GetComponent<ScalableObject>().layer;
				outlined = null;
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

        if (!moveOrInteract)
        {
			//Interact
			if (target.GetComponent<ScaleWithMouseWheel>())
			{
				target.GetComponent<ScaleWithMouseWheel>().Scalate();

				distance = Mathf.Clamp(distance, minDistance * (target.transform.localScale.x + target.transform.localScale.y + target.transform.localScale.z) / 3, maxDistance);
				grabber.position = transform.position + transform.forward * distance;
			}
        }

		//Set the grabber
		if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance, ignoreTargetMask))
		{
            if (moveOrInteract)
            {
				distance = Mathf.Clamp(distance + Input.GetAxis("Mouse ScrollWheel") * chasingSpeed, minDistance * (target.transform.localScale.x + target.transform.localScale.y + target.transform.localScale.z) / 3, distance);
			}
			grabber.position = hit.point;
        }
        else
        {
			if (moveOrInteract)
			{
				distance = Mathf.Clamp(distance + Input.GetAxis("Mouse ScrollWheel") * chasingSpeed, minDistance * (target.transform.localScale.x + target.transform.localScale.y + target.transform.localScale.z) / 3, maxDistance);
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
			//Si el jugador esta entre el objeto y el grabber
            if (Physics.Raycast(target.position, grabber.position - target.position, out RaycastHit hitted, distance))
            {
                if (hitted.collider.gameObject.layer == 3)
                {
					return;
				}
            }

			float angulo = Vector3.Angle(grabber.position - target.transform.position, transform.parent.position - target.transform.position);
			//Debug.Log(angulo);
			if (angulo < angle)
            {
				
				//Debug.DrawRay(target.transform.position, grabber.position - target.transform.position, Color.red);
				//Debug.DrawRay(target.transform.position, transform.parent.position - target.transform.position, Color.red);
				if (Vector3.Distance(grabber.position, transform.parent.position) < distancetoFly * (target.transform.localScale.x + target.transform.localScale.y + target.transform.localScale.z) / 3)
				{
					return;
				}
			}


			foreach (var caster in transform.parent.GetComponent<Player>().rayCasters)
			{
				float distance = caster.position.y - (transform.parent.position.y - 1.02f);
				//Debug.DrawRay(caster.position, -transform.parent.up * distance, Color.red);

				if (Physics.Raycast(caster.position, -transform.parent.up, out RaycastHit cube, distance))
				{
                    if (cube.collider.gameObject.layer == 12)
                    {
						//return;
					}
				}
				
			}
		}

		//Bloquea los muros azules
		//Debug.DrawRay(target.transform.position, grabber.position - target.transform.position, Color.red);
        if (Physics.Raycast(target.transform.position,grabber.position - target.transform.position, out RaycastHit obstacle, Vector3.Distance(grabber.position, target.transform.position), blockcubes))
        {
			//Se pega un poco al muro
			target.GetComponent<Rigidbody>().velocity = (obstacle.point - target.transform.position).normalized * Vector3.Distance(obstacle.point, target.transform.position) * chasingSpeed;
		}
		else
        {
			target.GetComponent<Rigidbody>().velocity = (grabber.position - target.transform.position).normalized * Vector3.Distance(grabber.position, target.transform.position) * chasingSpeed;
		}

		//Decall
        if (Physics.Raycast(target.transform.position, -Vector3.up, out RaycastHit decallPoint))
        {
			cubeShadow.transform.position = decallPoint.point + Vector3.up * 0.01f;

			//cubeShadow.GetComponent<DecalProjector>().size = new Vector2((target.transform.localScale.x + target.transform.localScale.y + target.transform.localScale.z) / 3, (target.transform.localScale.x + target.transform.localScale.y + target.transform.localScale.z) / 3);
			float mediumScale = (target.transform.localScale.x + target.transform.localScale.y + target.transform.localScale.z) / 3;
			cubeShadow.transform.localScale = new Vector3(mediumScale, mediumScale, mediumScale);
		}
	}

	void ChangeColor()
    {
		if (moveOrInteract)
		{
			rayVFX.SetBool("MoveOrScalate", true);
			sphereVFX.SetBool("MoveOrScalate", true);
			sphereMaterial.SetInt("_MoveScaleMode", 1);
			cubeShadow.GetComponent<DecalProjector>().material.SetFloat("_MoveOrScale", 1);//_MoveOrScale
			
			sphereLight.color = sphereMaterial.GetColor("_StarColorMove");
		}
		else
		{
			rayVFX.SetBool("MoveOrScalate", false);
			sphereVFX.SetBool("MoveOrScalate", false);
			sphereMaterial.SetInt("_MoveScaleMode", 0);
			cubeShadow.GetComponent<DecalProjector>().material.SetFloat("_MoveOrScale", 0);//_MoveOrScale
			
			sphereLight.color = sphereMaterial.GetColor("_StarColorScale");
		}
	}

	public void ChangeVolume()
    {
		AudioListener.volume = slid.value;
	}
}

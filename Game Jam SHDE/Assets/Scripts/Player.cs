using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{

    [Header("Movement")]

    Rigidbody rigi;
    
    //Movement
    Vector2 InputMovement;

    [SerializeField]
    [Tooltip("")]
    float movementSpeed;

    [SerializeField]
    [Tooltip("")]
    float movementSpeedInAir;

    [SerializeField]
    [Tooltip("")]
    float rampSpeedMultiplier;

    [SerializeField]
    [Tooltip("")]
    float speedLimit;

    [SerializeField]
    Vector3 actualSpeed;
    [SerializeField]
    Vector3 inputSpeed;

    [Space(20)]

    //Camera
    Vector2 inputRotation;
    float camRotX;
    Transform cam;

    [Space(20)]
    //Cambialo con un metodo
    public float mouseSensibility;

    [SerializeField]
    float maxCamRotationUp;

    [SerializeField]
    float maxCamRotationDown;

    [Space(20)]
    //Jump
    [SerializeField]
    float jumpForce;

    [SerializeField]
    LayerMask jumpLayers;

    [Tooltip("The list of casters that check if the player is touching the ground")]
    public List<Transform> rayCasters;

    [SerializeField]
    float falseGravity;

    [SerializeField]
    bool grounded;

    float angle;

    [Header("Animation")]
    [SerializeField]
    Animator staffAnimationController;

    [Header("Menu")]
    [SerializeField]
    bool menuactivated;
    [SerializeField]
    GameObject menu;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Set variables
        rigi = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        camRotX = cam.rotation.x;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetMenu();
        }

        //Detect Input
        InputMovement = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        inputRotation = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (InputMovement != Vector2.zero)
        {
            staffAnimationController.SetBool("Walking", true);
        }
        else
        {
            staffAnimationController.SetBool("Walking", false);
        }
        

        //Player is in the ground
        bool jumped = false;
        bool groundDetected = false;
        
        foreach (var caster in rayCasters)
        {
            float distance = caster.position.y - (transform.position.y - 1.02f);
            //Debug.DrawRay(caster.position, -transform.up * distance, Color.red);

            if (Physics.Raycast(caster.position, -transform.up, out RaycastHit floor, distance, jumpLayers))
            {
                groundDetected = true;
                grounded = true;
                
                angle = Vector3.Angle(floor.normal, transform.up);

                //Jump
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!jumped)
                    {
                        rigi.isKinematic = false;

                        rigi.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                        jumped = true;

                        //quitar hijo de puente
                        transform.SetParent(null);
                    }
                }

                staffAnimationController.SetBool("Falling", false);

                //Hacer hijo de puente
                if (floor.collider.gameObject.layer == 11)
                {
                    transform.SetParent(floor.collider.gameObject.transform);
                }
            }
            else
            {
                if (rigi.velocity.y < 0)
                {
                    staffAnimationController.SetBool("Falling", true);
                }

                if (!groundDetected)
                {
                    grounded = false;
                }
            }
        }

        //Lo ponemos kinematiko
        if (grounded)
        {
            if (InputMovement != Vector2.zero)
            {
                rigi.isKinematic = false;
            }
            else
            {
                //rigi.isKinematic = true;
                //revisar si es kinematik para saber si lo hemos puesto en este sitio
            }
        }
    }

    private void FixedUpdate()
    {
        //Move and rotate the player
        if (grounded)
        {
            

            //angulo para escalar rapido
            float multiplier = Mathf.Clamp(angle / 60, 0, 1)* rampSpeedMultiplier;
            if (multiplier < 1)
            {
                multiplier = 1;
            }

            /*
            if (InputMovement == Vector2.zero)
            {
                actualSpeed = new Vector3(rigi.velocity.x, 0, rigi.velocity.z); //rigi.velocity;
                actualSpeed = rigi.velocity; //rigi.velocity;
            }
            
            inputSpeed = ((transform.forward * InputMovement.x) + (transform.right * InputMovement.y)) * movementSpeed * Time.fixedDeltaTime * multiplier;
            rigi.velocity = Vector3.ClampMagnitude(actualSpeed + inputSpeed, speedLimit);

            actualSpeed = new Vector3(rigi.velocity.x, 0, rigi.velocity.z) - inputSpeed;
            actualSpeed = rigi.velocity - inputSpeed;
            */

            //Sumarle inputs a la velocidad
            //Porblema: Deslizamiento
            //rigi.velocity = Vector3.ClampMagnitude(rigi.velocity + (((transform.forward * InputMovement.x) + (transform.right * InputMovement.y)) * movementSpeed * Time.fixedDeltaTime * multiplier), speedLimit);

            //La velocidad son los inputs
            //Problema: No te pegas al puente
            rigi.velocity = ((transform.forward * InputMovement.x) + (transform.right * InputMovement.y)) * movementSpeed * Time.fixedDeltaTime * multiplier;
        }
        else
        {
            rigi.velocity = ((transform.forward * InputMovement.x) + (transform.right * InputMovement.y)) * movementSpeedInAir * Time.fixedDeltaTime + new Vector3(0, rigi.velocity.y, 0);
        }
        



        transform.Rotate(0, inputRotation.x * mouseSensibility, 0);// * Time.deltaTime

        //Rotate the Camera
        camRotX -= inputRotation.y * mouseSensibility;// * Time.deltaTime
        camRotX = Mathf.Clamp(camRotX, -maxCamRotationUp, maxCamRotationDown);

        
        cam.localRotation = Quaternion.Euler(camRotX, cam.localRotation.y, cam.localRotation.z);
        rigi.AddForce(-Vector3.up * falseGravity, ForceMode.Force);
        
    }

    void Death()
    {
        SceneManager.LoadScene(0);
    }

    //check if your height is very low, so you went through the ground somehow

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            Death();
        }
    }

    public void SetMenu()
    {
        if (menuactivated)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            Time.timeScale = 1;

            menu.SetActive(false);
            menuactivated = false;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 0;

            menu.SetActive(true);
            menuactivated = true;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}

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

    [Space(20)]
    //Camera
    Vector2 inputRotation;
    float camRotX;
    Transform cam;
    
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

    [SerializeField]
    [Tooltip("The list of casters that check if the player is touching the ground")]
    List<Transform> rayCasters;

    [SerializeField]
    float falseGravity;

    [SerializeField]
    bool grounded;

    float angle;

    //Coyote Time
    /*
    float timeInAir;

    [SerializeField]
    float coyoteTime;

    bool jumped = false;
    */
    //Input buffer
    /*
    Queue<KeyCode> inputBuffer = new Queue<KeyCode>();
    */

    [Header("Animation")]
    [SerializeField]
    Animator staffAnimationController;

    void Start()
    {
        //Set the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Set variables
        rigi = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        camRotX = cam.rotation.x;

        StartCoroutine("VerifyHigh");
    }
    
    void Update()
    {
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
            Debug.DrawRay(caster.position, -transform.up * distance, Color.red);

            if (Physics.Raycast(caster.position, -transform.up, out RaycastHit raycastHit, distance, jumpLayers))
            {
                groundDetected = true;
                grounded = true;

                //
                //Debug.Log(Vector3.Angle(raycastHit.normal, transform.up));
                angle = Vector3.Angle(raycastHit.normal, transform.up);


                //Jump
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!jumped)
                    {
                        rigi.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                        jumped = true;

                    }
                }

                staffAnimationController.SetBool("Falling", false);
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

        #region CoyoteAndInput
        /*
        if (Physics.Raycast(transform.position - transform.up * 0.95f, -transform.up, out RaycastHit groundRaycast, 0.08f, jumpLayers))
        {
            
            jumped = false;
            if (inputBuffer.Count > 0)
            {
                rigi.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                InputDeQueue();

                jumped = true;
            }

            //timeInAir = 0;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //inputBuffer.Enqueue(KeyCode.Space);
                //Invoke("InputDeQueue", 5);

                //Debug.Log(1 + "    " + inputBuffer.Count);

                rigi.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            }

            staffAnimationController.SetBool("Falling", false);
        }
        else
        {
            
            timeInAir += Time.deltaTime;
            if (timeInAir < coyoteTime)// && !jumped
            {
                if (inputBuffer.Count > 0)
                {
                    rigi.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                    InputDeQueue();

                    jumped = true;
                }
            }
            
            if (rigi.velocity.y < 0)
            {
                staffAnimationController.SetBool("Falling", true);
            }
        }
        */
        #endregion
    }

    private void FixedUpdate()
    {
        //Move and rotate the player
        if (grounded)
        {
            //angulo para escalar rapido
            float multiplier = 0;
            if (angle / 60 < 1)
            {
                multiplier = angle / 60;
            }
            else
            {
                multiplier = 1;
            }
            multiplier *= 2;
            if (multiplier < 1)
            {
                multiplier = 1;
            }

            rigi.velocity = ((transform.forward * InputMovement.x) + (transform.right * InputMovement.y)) * movementSpeed * Time.fixedDeltaTime * multiplier;// + new Vector3(0, rigi.velocity.y, 0)
        }
        else
        {
            rigi.velocity = ((transform.forward * InputMovement.x) + (transform.right * InputMovement.y)) * movementSpeed * Time.fixedDeltaTime + new Vector3(0, rigi.velocity.y, 0);
        }
        transform.Rotate(0, inputRotation.x * mouseSensibility, 0);// * Time.deltaTime

        //Rotate the Camera
        camRotX -= inputRotation.y * mouseSensibility;// * Time.deltaTime
        camRotX = Mathf.Clamp(camRotX, -maxCamRotationUp, maxCamRotationDown);

        cam.localRotation = Quaternion.Euler(camRotX, cam.localRotation.y, cam.localRotation.z);
        //cam.Rotate(-inputRotation.y * mouseSensibility, 0, 0);
        rigi.AddForce(-Vector3.up * falseGravity, ForceMode.Force);
    }

    /*
    void InputDeQueue()
    {
        if (inputBuffer.Count > 0)
        {
            inputBuffer.Dequeue();
        }
    }
    */



    void Death()
    {
        SceneManager.LoadScene(0);
    }

    //check if your height is very low, so you went through the ground somehow
    IEnumerator VerifyHigh()
    {
        if (transform.position.y < -50)
        {
            Death();
        }
        yield return new WaitForSeconds(5);
        StartCoroutine("VerifyHigh");
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            Death();
        }
    }
}

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

    float timeInAir;

    [SerializeField]
    float coyoteTime;

    bool jumped = false;

    Queue<KeyCode> inputBuffer = new Queue<KeyCode>();

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputBuffer.Enqueue(KeyCode.Space);
            //Invoke("InputDeQueue", 5);

            Debug.Log(1 + "    " + inputBuffer.Count);
        }
        //Player is in the ground
        if (Physics.Raycast(transform.position - transform.up * 0.95f, -transform.up, out RaycastHit groundRaycast, 0.08f, jumpLayers))
        {
            jumped = false;
            if (inputBuffer.Count > 0)
            {
                rigi.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                InputDeQueue();

                jumped = true;
            }

            timeInAir = 0;

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
    }

    private void FixedUpdate()
    {
        //Move and rotate the player
        rigi.velocity = ((transform.forward * InputMovement.x) + (transform.right * InputMovement.y)) * movementSpeed * Time.fixedDeltaTime + new Vector3(0,rigi.velocity.y,0);
        transform.Rotate(0, inputRotation.x * mouseSensibility, 0);

        //Rotate the Camera
        camRotX -= inputRotation.y * mouseSensibility;
        camRotX = Mathf.Clamp(camRotX, -maxCamRotationUp, maxCamRotationDown);

        cam.localRotation = Quaternion.Euler(camRotX, cam.localRotation.y, cam.localRotation.z);
        //cam.Rotate(-inputRotation.y * mouseSensibility, 0, 0);

    }

    void InputDeQueue()
    {
        if (inputBuffer.Count > 0)
        {
            inputBuffer.Dequeue();
        }
    }

    //check if your height is very low, so you went through the ground somehow
    IEnumerator VerifyHigh()
    {
        if (transform.position.y < -50)
        {
            SceneManager.LoadScene(0);
        }
        yield return new WaitForSeconds(5);
        StartCoroutine("VerifyHigh");
    }
    


}

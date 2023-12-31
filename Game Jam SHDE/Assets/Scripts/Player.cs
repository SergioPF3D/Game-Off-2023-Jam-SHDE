using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    float sliderSensibility = 1;

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

    [Header("Audio")]

    [SerializeField]
    AudioSource audios;

    [SerializeField]
    List<AudioClip> footsteps;

    [SerializeField]
    float timeSounds;

    float basePitch;
    [SerializeField]
    float audiovariation;

    float baseVolume;
    [SerializeField]
    float volumeVariation;

    //[SerializeField]
    //AudioClip fallingInVoid;

    [Header("UI")]
    [SerializeField]
    Slider slid;

    public bool blockUI;
    public bool blockMovement;

    float timeafterJump;


    [SerializeField]
    MoveObjects mover;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Set variables
        rigi = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        camRotX = cam.rotation.x;

        basePitch = audios.pitch;
        baseVolume = audios.volume;

        StartCoroutine(PlayFootsTeps());

        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            //slid.value = PlayerPrefs.GetFloat("Sensitivity");
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !blockUI)
        {
            SetMenu(menuactivated);
        }

        //Detect Input
        if (!blockMovement)
        {
            InputMovement = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
            inputRotation = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        if (InputMovement != Vector2.zero)
        {
            staffAnimationController.SetBool("Walking", true);
        }
        else
        {
            staffAnimationController.SetBool("Walking", false);
        }
        
        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                //rigi.isKinematic = false;

                rigi.AddForce(0, jumpForce, 0, ForceMode.Impulse);

                //quitar hijo de puente
                transform.SetParent(null);

                staffAnimationController.SetTrigger("Jump");
                staffAnimationController.SetBool("Grounded", false);

                //StopCoroutine(PlayFootsTeps());
                //StopCoroutine("PlayFootsTeps");
                StopAllCoroutines();

                timeafterJump = 0;
            }
        }
        else
        {

            bool groundDetected = false;

            foreach (var caster in rayCasters)
            {
                float distance = caster.position.y - (transform.position.y - 1.015f); //0.2f;
                Debug.DrawRay(caster.position, -transform.up * distance, Color.red);

                if (Physics.Raycast(caster.position, -transform.up, out RaycastHit floor, distance, jumpLayers))
                {
                    groundDetected = true;
                    if (!grounded && rigi.velocity.y < 0)
                    {
                        //Mejor que dependa de la altura de la que cae, es decir su velocidad en y
                        //PlaySound();

                    }

                    grounded = true;
                    staffAnimationController.SetBool("Grounded", true);

                    //audios.PlayOneShot(footsteps[0],0.75f);

                    StopAllCoroutines();
                    StartCoroutine(PlayFootsTeps());

                    angle = Vector3.Angle(floor.normal, transform.up);



                    staffAnimationController.SetBool("Falling", false);

                    if (mover.target)
                    {
                        if (floor.collider.gameObject == mover.target.gameObject)
                        {
                            mover.blocked = true;
                        }
                        else
                        {
                            mover.blocked = false;
                        }
                    }

                    /*
                    //Hacer hijo de puente
                    if (floor.collider.gameObject.layer == 11)
                    {
                        transform.SetParent(floor.collider.gameObject.transform);
                    }
                    */
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

            if (timeafterJump > 0.2f)
            {
                
            }
            else
            {
                //grounded = false;
                //timeafterJump += Time.fixedDeltaTime;
                //Debug.Log(timeafterJump);
            }
        }

        

        //Lo ponemos kinematiko
        /*
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
        */
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

            if (timeafterJump > 0.2f)
            {
                rigi.velocity = ((transform.forward * InputMovement.x) + (transform.right * InputMovement.y)) * movementSpeed * Time.fixedDeltaTime * multiplier;  //+ new Vector3(0, rigi.velocity.y, 0); 

            }
            else
            {
                //grounded = false;
                timeafterJump += Time.fixedDeltaTime;
                //Debug.Log(timeafterJump);


                rigi.velocity = ((transform.forward * InputMovement.x) + (transform.right * InputMovement.y)) * movementSpeedInAir * Time.fixedDeltaTime + new Vector3(0, rigi.velocity.y, 0);

            }


        }
        else
        {
            rigi.velocity = ((transform.forward * InputMovement.x) + (transform.right * InputMovement.y)) * movementSpeedInAir * Time.fixedDeltaTime + new Vector3(0, rigi.velocity.y, 0);
        }
        
        //Mirar por ahi
        transform.Rotate(new Vector3(0, inputRotation.x, 0) * Time.fixedDeltaTime * mouseSensibility * sliderSensibility);
        //Rotate the Camera
        camRotX -= inputRotation.y * Time.fixedDeltaTime * mouseSensibility * sliderSensibility;
        camRotX = Mathf.Clamp(camRotX, -maxCamRotationUp, maxCamRotationDown);
        cam.localRotation = Quaternion.Euler(camRotX, cam.localRotation.y, cam.localRotation.z);
    }

    void Death()
    {
        //audios.PlayOneShot(fallingInVoid,1);
        SceneManager.LoadScene(1);
    }

    //check if your height is very low, so you went through the ground somehow

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            Death();
        }
    }

    public void SetMenu(bool activated)
    {
        
        if (activated)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            Time.timeScale = 1;

            menu.SetActive(false);
            menuactivated = false;

            
            if (PlayerPrefs.HasKey("Volume"))
            {
                AudioListener.volume = PlayerPrefs.GetFloat("Volume");
            }
            else
            {
                AudioListener.volume = 0.5f;
            }
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 0;

            menu.SetActive(true);
            menuactivated = true;

            AudioListener.volume = 0;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    IEnumerator PlayFootsTeps()
    {
        if (InputMovement != Vector2.zero)//grounded && 
        {
            PlaySound(); 
        }
        yield return new WaitForSeconds(timeSounds);
        StartCoroutine(PlayFootsTeps());
    }
    
    void PlaySound()
    {
        audios.clip = footsteps[Random.Range(1, footsteps.Count)];
        
        audios.volume = baseVolume + Random.Range(-volumeVariation / 100 * baseVolume, volumeVariation / 100 * baseVolume);
        audios.pitch = basePitch + Random.Range(-audiovariation / 100 * basePitch, audiovariation / 100 * basePitch);
        audios.Play();       
    }

    public void ChangeSensibility()
    {
        if (slid.value > 0)
        {
            sliderSensibility = Mathf.Lerp(1,10, Mathf.Abs(slid.value) / 10);
        }
        else if (slid.value < 0)
        {
            sliderSensibility = Mathf.Lerp(1, 0.1f, Mathf.Abs(slid.value) / 10);
        }
        else
        {
            sliderSensibility = 1;
        }
        PlayerPrefs.SetFloat("Sensitivity", slid.value);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FinalVolume")
        {
            this.gameObject.GetComponentInChildren<MoveObjects>().StartCoroutine("SeeDistanceToFinal");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "FinalVolume")
        {
            this.gameObject.GetComponentInChildren<MoveObjects>().StopCoroutine("SeeDistanceToFinal");
        }
    }
}

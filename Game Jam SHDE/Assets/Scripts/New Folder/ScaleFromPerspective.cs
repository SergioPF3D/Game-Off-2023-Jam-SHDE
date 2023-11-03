using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromPerspective : MonoBehaviour
{
    [SerializeField]
    Transform ObjectToMove;

    //Scalate Objects
    [SerializeField]
    LayerMask targetMask;
    [SerializeField]
    LayerMask ignoreTargetMask;

    [SerializeField]
    float offsetFactor;

    [SerializeField]
    float distanceToScale; //Si no toca nada, que se ponga a la distance to scale

    //Move
    [SerializeField]
    Rigidbody rigi;

    [SerializeField]
    float movementSpeed;

    [SerializeField]
    float jumpForce;

    [SerializeField]
    float rotationSpeed;

    [SerializeField]
    Camera cam;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {

        

        /*
        if (Input.GetMouseButtonDown(0))
        {
            //Seteamos el objeto
             
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitinfo, 10000, targetMask))
            {
                ObjectToMove = hitinfo.collider.transform;
                ObjectToMove.GetComponent<Rigidbody>().isKinematic = true;
                originalDistance = Vector3.Distance(cam.transform.position, ObjectToMove.position);
                originalScale = ObjectToMove.localScale;
                targetScale = ObjectToMove.localScale;
            }
        }
        if (Input.GetMouseButton(0))
        {

            if (ObjectToMove != null)
            {


                //Escalamos
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitinfo, ignoreTargetMask))
                {
                    //Que no sea solo la x
                    ObjectToMove.position = hitinfo.point - cam.transform.forward * offsetFactor * targetScale.x;

                    float distance = Vector3.Distance(cam.transform.position, ObjectToMove.position);
                    float relation = distance / originalDistance;

                    //Cambiar esto para que sea con objetos que estan escalados en un eje
                    targetScale.x = targetScale.y = targetScale.z = distance;

                    ObjectToMove.transform.localScale = targetScale * originalDistance;

                }
 
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (ObjectToMove != null)
            {
                ObjectToMove.GetComponent<Rigidbody>().isKinematic = false;

                ObjectToMove = null;
            }
        }
        */

        #region Movement
        
        
        if (Input.GetKey(KeyCode.W))
        {
            rigi.MovePosition(transform.position + transform.forward * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigi.MovePosition(transform.position + transform.right * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rigi.MovePosition(transform.position + -transform.right * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rigi.MovePosition(transform.position + -transform.forward * movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            rigi.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        

        //rigi.MovePosition(transform.position + new Vector3(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime));
        
        /*
        if (Input.GetAxis("Mouse X") != 0)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed, 0, Space.Self);
        }

        if (Input.GetAxis("Mouse Y") != 0)
        {
            cam.transform.Rotate(-Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed, 0, 0, Space.Self);
        }
        */
        
        //rigi.velocity = new Vector3(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime, rigi.velocity.y, Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime);




        cam.transform.Rotate(-Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed, Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed, 0, Space.Self);

        #endregion
    }
}

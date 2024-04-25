using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float speed = 1.0f;
    [SerializeField] float grav = -9.81f;
    [SerializeField] float floatingDistance = 1.5f;
    [SerializeField] float camSensX = 1.0f;
    [SerializeField] float camSensY = 1.0f;

    

    //Objects
    PlayerActionControls pc;
    Rigidbody rb;
    Camera cam;

    //Variables
    LayerMask isGround;


    void Awake()
    {   
        
        pc = new PlayerActionControls();
        pc.Movement.WASD.Enable();
        pc.Movement.LookAround.Enable();

        rb = gameObject.GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();

        // isGround = LayerMask.NameToLayer("Ground");
        isGround = 1 <<  LayerMask.NameToLayer("Ground");
        Debug.Log(LayerMask.NameToLayer("Ground"));
        // isGround;

    }


    // Update is called once per frame
    void Update()
    {
        // print("wtff");
        // Debug.Log("Doges this dogert");
        
        // checkGroundDist();
    }

    void FixedUpdate()
    {

        Vector3 gravity = new Vector3(0, grav, 0);
        
        //Movement and ground distance
        Move();
        checkGroundDist();
        

        //Camera Looking with Mouse:
        moveCamera();
        
    }

    private void Move(){  
        Vector2 rawInput = pc.Movement.WASD.ReadValue<Vector2>();
        // Debug.Log(rawInput);
        Vector3 input = new Vector3(rawInput.x, 0, rawInput.y);
        
        rb.velocity = input * speed;

        //Check to Stay on the ground
    }

    private void changeGrav(float newGrav){
        grav = newGrav;
    }

    private void checkGroundDist(){
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, isGround))
        {
            // Debug.Log("Did Hit");            
            // if(hit.distance - floatingDistance < 0) transform.position += new Vector3(0.0f, floatingDistance - hit.distance, 0.0f);
            transform.position += new Vector3(0.0f, floatingDistance - hit.distance, 0.0f);
        }

    }

    private void moveCamera(){
        Vector2 rawInput = pc.Movement.LookAround.ReadValue<Vector2>();
        Debug.Log(rawInput);


        //Translating x direction to PLAYER y rotation
        transform.Rotate(0.0f, rawInput.x * camSensX, 0.0f, Space.Self);

        //Translating y direction to camera z rotation
        cam.transform.Rotate(-rawInput.y * camSensY, 0.0f, 0.0f, Space.Self);

    }
}

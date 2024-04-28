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
    // [SerializeField] float camDegLimit = 90.0f;
    [SerializeField] float timeScale = 1.0f;

    

    //Objects
    PlayerActionControls pc;
    Rigidbody rb;
    Camera cam;

    int itemLayerMask = 1 << 7; // huh.
    GameObject leftHand = null, rightHand = null; // what either hand is carrying

    //Variables
    LayerMask isGround;


    void Awake()
    {   
        
        pc = new PlayerActionControls();
        pc.Movement.WASD.Enable();
        pc.Movement.LookAround.Enable();
        pc.Movement.Click.Enable();
        pc.Movement.Click.performed += _ => PickUp();

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
        Time.timeScale = timeScale;
        

        //Camera Looking with Mouse:
        moveCamera();
        
    }

    private void Move(){  
        Vector2 rawInput = pc.Movement.WASD.ReadValue<Vector2>();
        // Debug.Log(rawInput);
        // Vector3 input = new Vector3(rawInput.x, 0, rawInput.y);
        Vector3 input = transform.forward*rawInput.y + transform.right*rawInput.x;
        
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

        //Translating x direction to PLAYER y rotation
        transform.Rotate(0.0f, rawInput.x * camSensX, 0.0f, Space.Self);

        //Translating y direction to camera x rotation
        cam.transform.Rotate(-rawInput.y * camSensY, 0.0f, 0.0f, Space.Self);


        float currentPitch = cam.transform.localEulerAngles.x;
        // Debug.Log("Current Pitch: " + currentPitch);
        //For some reason 0/360 is the beginning angle which makes sense but it's also b/t -180 — +180??? on the documentation???

        //Checking head pitch angle (see pitch, yaw, roll)
        // if(currentPitch > camDegLimit && currentPitch < 180.0f){
        //     Debug.Log("Too high");
        //     // cam.transform.Rotate(-(currentPitch - camDegLimit), 0.0f, 0.0f, Space.Self);
        // }else if(currentPitch > 180.0f && currentPitch < 360.0f - camDegLimit){
        //     Debug.Log("too Low");
        //     // cam.transform.Rotate( (360.0f - camDegLimit) -currentPitch, 0.0f, 0.0f, Space.Self);
        // }

        //Dot the player "forward" and the camera forward. If it's negative, then it's too far.
        //Then, check if cam forward dot with Player up is postivie or negative, and adjust accordingly.
        
        if(Vector3.Dot(cam.transform.forward,  transform.forward) < 0){ //If turnaround
            Quaternion temp;
            Vector3 rotation;

            if(Vector3.Dot(cam.transform.forward, transform.up) >= 0){ //If too high
                Debug.Log("Too High");
                temp = Quaternion.FromToRotation(cam.transform.forward, transform.up);
                rotation = temp.eulerAngles;
                cam.transform.Rotate(new Vector3(rotation.x, 0.0f, 0.0f));
                Debug.Log("Adjusting with: " + rotation.x);
            }else{ //If too low
                Debug.Log("Too low");
                temp = Quaternion.FromToRotation(cam.transform.forward, -transform.up);
                rotation = temp.eulerAngles;
                cam.transform.Rotate(new Vector3(rotation.x, 0.0f, 0.0f));
                Debug.Log("Adjusting with: " + rotation.x);
            }



        }


    }

    private void PickUp() {
        Transform t = cam.GetComponent<Transform>();
        Vector3 pos = t.position;
        Vector3 dir = t.TransformDirection(Vector3.forward);
        RaycastHit hit;

        // origin, direction, where to put the raycast, distance to cast, layer
        bool lookingAtObject = Physics.Raycast(pos, dir, out hit, 20, itemLayerMask);
        Debug.DrawRay(pos, dir, Color.red, 10);

        if(!lookingAtObject)
        {
            Debug.Log("Pressed left click (pick up), not looking at/close enough to object");
            return; // ha ha
        } 
        Debug.Log("We picked up an object!!!");
        leftHand = hit.collider.gameObject; // set the object being held

        // pick up an item, i guess???

    }
}


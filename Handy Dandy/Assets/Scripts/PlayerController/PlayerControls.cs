using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerControls : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float speed = 1.0f;
    [SerializeField] float grav = -9.81f;
    [SerializeField] float groundCheckDistance = .3f;
    [SerializeField] float camSensX = 1.0f;
    [SerializeField] float camSensY = 1.0f;
    [SerializeField] float camDegLimit = 90.0f;
    [SerializeField] float roomForError = 1.0f;
    [SerializeField] float timeScale = 1.0f;
    [SerializeField] float sprintMult = 1.0f;
    [SerializeField] float jumpPower = 5.0f;

    //Objects
    PlayerActionControls pc;
    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    Camera cam;
    [SerializeField] UIManager ui;

    bool lookingAtObject = false;    
    RaycastHit hit;
    int itemLayerMask = 1 << 7; // huh.
    GameObject leftHand = null, rightHand = null; // what either hand is carrying

    GameObject leftHand, rightHand; // what either hand is carrying
    //Variables
    LayerMask isGround;
    float currentCamRotation = 0.0f;
    bool isGrounded = false;
    float slopeAngle;
    float playerheight;
    RaycastHit slopeHit;

    void Awake()
    {   
        pc = new PlayerActionControls();
        pc.Movement.Enable();
        pc.Movement.WASD.Enable();
        pc.Movement.WASD.canceled += _ => Stop();
        pc.Movement.LookAround.Enable();
        // pc.Movement.Click.Enable();
        // pc.Movement.Click.performed += _ => PickUp();
        pc.Movement.Sprint.performed += _ => Sprint(true);
        pc.Movement.Sprint.canceled += _ => Sprint(false);
        pc.Movement.Jump.performed += _ => Jump();


        pc.Movement.LClick.Enable();
        pc.Movement.RClick.Enable();

        pc.Movement.LClick.performed += _ => PickUp();
        pc.Movement.RClick.performed += _ => Drop();

        rb = gameObject.GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
        playerheight = capsuleCollider.height;

        isGround = 1 <<  LayerMask.NameToLayer("Ground");
        
        Debug.Log(LayerMask.NameToLayer("Ground"));
        // isGround;

        leftHand = rightHand = null;

    }

    // Update is called once per frame
    void Update()
    {
        // playerheight / 2 = bottom of capsule since transform is at exact center
        if (Physics.Raycast(transform.position, Vector3.down, (playerheight / 2) + groundCheckDistance))
        {
            isGrounded = true;
            Debug.Log("Grounded");
        }
        else
        {
            isGrounded = false;
            Debug.Log("Not Grounded");

        }
        Transform t = cam.GetComponent<Transform>();
        Vector3 pos = t.position;
        Vector3 dir = t.TransformDirection(Vector3.forward);

        // origin, direction, where to put the raycast, distance to cast, layer
        lookingAtObject = Physics.Raycast(pos, dir, out hit, 1000, itemLayerMask);
        Debug.DrawRay(pos, dir, Color.red, 10);

        if(leftHand == null) {
            Debug.Log("leftHand empty");
            ui.Drop();
            if(lookingAtObject) {
                ui.Point();
            }
            else {
                ui.Idle();
            }
        }
        else {
            ui.Hold();
            if(leftHand.name == "Apple") {
                ui.HoldApple();
            }
        }

        // if(lookingAtObject && leftHand == null) {
        //     ui.Point();
        // }
        // else if(leftHand == null)
        // {
        //     ui.Idle();
        // }
        // else
        // {
        //     ui.Hold();
        //     // spawn the stupid 3d item for the stupid hold ui
        // }


        // print("wtff");
        // Debug.Log("Doges this dogert");
        
        // checkGroundDist();
    }

    void FixedUpdate()
    {

        //Vector3 gravity = new Vector3(0, grav, 0);

        if (!isGrounded)
        {
            rb.AddForce(Physics.gravity * grav, ForceMode.Acceleration);
        }

        //Movement and ground distance
        Move();
        //checkGroundDist();
        //Time.timeScale = timeScale;
        

        //Camera Looking with Mouse:
        moveCamera();
    }

    private void Move(){
        // Notable: I used 10 to estimate player height, but should find this manually incase we scale character
        if (pc.Movement.WASD.IsPressed())
        {
            Vector3 input;
            Physics.Raycast(transform.position, Vector3.down, out slopeHit, Mathf.Infinity, isGround);
            Vector2 rawInput = pc.Movement.WASD.ReadValue<Vector2>();
            input = Vector3.ProjectOnPlane(transform.forward * rawInput.y + transform.right * rawInput.x, slopeHit.normal);
            rb.transform.position += input.normalized * speed;
        }
    }

    void Stop()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    private void changeGrav(float newGrav){
        grav = newGrav;
    }

    //private void checkGroundDist(){
    //    RaycastHit hit;

    //    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, isGround))
    //    {
    //        transform.position += new Vector3(0.0f, floatingDistance - hit.distance, 0.0f);
    //    }

    //}

    private void moveCamera(){
        Vector2 rawInput = pc.Movement.LookAround.ReadValue<Vector2>(); //Moving up is pos, down is negative
        float verticalChange = -rawInput.y * camSensY;

        //Translating x direction to PLAYER y rotation
        transform.Rotate(0.0f, rawInput.x * camSensX, 0.0f, Space.Self);

        //Translating y direction to camera x rotation
        currentCamRotation += verticalChange;
        if(currentCamRotation > camDegLimit + roomForError){
            verticalChange = camDegLimit - (currentCamRotation-verticalChange);
            cam.transform.Rotate(verticalChange, 0.0f, 0.0f, Space.Self);
            currentCamRotation = camDegLimit;

        }else if(currentCamRotation < -camDegLimit - roomForError){
            verticalChange = -camDegLimit - (currentCamRotation-verticalChange);
            cam.transform.Rotate(verticalChange, 0.0f, 0.0f, Space.Self); 
            currentCamRotation = -camDegLimit;
        }else{
            cam.transform.Rotate(verticalChange, 0.0f, 0.0f, Space.Self);    
        }
        


    }

    private void PickUp() {
        Transform t = cam.GetComponent<Transform>();
        Vector3 pos = t.position;
        Vector3 dir = t.TransformDirection(Vector3.forward);
        RaycastHit hit;

        // origin, direction, where to put the raycast, distance to cast, layer
        bool lookingAtObject = Physics.Raycast(pos, dir, out hit, 20, itemLayerMask);
        //Debug.DrawRay(pos, dir, Color.red, 10);

        
        if(!lookingAtObject)
        {
            //Debug.Log("Pressed left click (pick up), not looking at/close enough to object");
            return; // ha ha
        } 
        //Debug.Log("We picked up an object!!!");
        leftHand = hit.collider.gameObject; // set the object being held
        // Destroy(hit.collider.gameObject);
        leftHand.SetActive(false);
        Debug.Log("We picked up " + leftHand.name);

        // pick up an item, i guess???

    }

    void Sprint(bool b){
        if(b){
            speed *= sprintMult;
        }else{
            speed = speed/sprintMult;
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
    
    private void Drop() {
        leftHand.SetActive(true);
        Transform t1 = cam.GetComponent<Transform>();
        leftHand.transform.position = new Vector3(t1.position.x, t1.position.y, t1.position.z) + transform.rotation * Vector3.forward;
        leftHand = null;
        ui.Idle();  
    }
}
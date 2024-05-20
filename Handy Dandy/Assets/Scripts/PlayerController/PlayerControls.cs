using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerControls : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float speed = 1.0f;
    [SerializeField] float grav = 1;
    [SerializeField] float groundCheckDistance = .3f;
    [SerializeField] float camSensX = 1.0f;
    [SerializeField] float camSensY = 1.0f;
    [SerializeField] float camDegLimit = 90.0f;
    [SerializeField] float roomForError = 1.0f;
    [SerializeField] float timeScale = 1.0f;
    [SerializeField] float sprintMult = 1.0f;
    [SerializeField] float jumpPower = 5.0f;
    [SerializeField] static int inventorySize = 10;

    //Objects
    PlayerActionControls pc;
    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    Camera cam;
    [SerializeField] UIManager ui;

    bool lookingAtObject = false;    
    RaycastHit hit;
    int itemLayerMask = 1 << 7; // huh.

    public enum hand{Left, Right};
    hand controllingHand = hand.Left;

    GameObject leftHand, rightHand; // what either hand is carrying
    //Variables
    LayerMask isGround;
    float currentCamRotation = 0.0f;
    bool isGrounded = false;
    float slopeAngle;
    float playerheight;
    RaycastHit slopeHit;
    Stack rightHandInventory = new Stack(inventorySize);
    Stack leftHandInventory = new Stack(inventorySize);
    int currentInventoryCapacityLeft = 5, currentInventoryCapacityRight = 5;
    int amountOfItemsHeldLeft = 0, amountOfItemsHeldRight = 0;
    string appleRegex = @"Apple.*", ice_cubeRegex = @"Ice_Cube.*";

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
        pc.Movement.SwitchHand.performed += _ => SwitchHand();

        rb = gameObject.GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
        playerheight = capsuleCollider.height;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        isGround = 1 <<  LayerMask.NameToLayer("Ground");
        
        Debug.Log(LayerMask.NameToLayer("Ground"));
        // isGround;

        leftHand = rightHand = null;

        HandleQuests.player = this;
    }

    // Update is called once per frame
    void Update()
    {
        // playerheight / 2 = bottom of capsule since transform is at exact center
        if (Physics.Raycast(transform.position, Vector3.down, (playerheight / 2) + groundCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        Transform t = cam.GetComponent<Transform>();
        Vector3 pos = t.position;
        Vector3 dir = t.TransformDirection(Vector3.forward);

        // origin, direction, where to put the raycast, distance to cast, layer
        lookingAtObject = Physics.Raycast(pos, dir, out hit, 1000, itemLayerMask);
        Debug.DrawRay(pos, dir, Color.red, 10);

        // if(leftHand == null) { ui.Idle(hand.Left); ui.Drop(hand.Left); }
        // if(rightHand == null) { ui.Idle(hand.Right); ui.Drop(hand.Right); }
        // if((controllingHand == hand.Left && leftHand == null) || (controllingHand == hand.Right && rightHand == null)) // If controlling hand is empty...
        // {
        //     // if the controlling hand is empty
        //     if(lookingAtObject) {
        //         ui.Point(controllingHand);
        //     }
        //     else {
        //         ui.Idle(controllingHand);
        //     }
        // }
        // else //Else if holding something
        // {
        //     ui.Hold(controllingHand); //Changes to holding.

        //     if(controllingHand == hand.Left)
        //     {
        //         if(Regex.Match(leftHand.name, appleRegex).Success){
        //             ui.HoldApple(controllingHand);
        //         }
        //         else if(Regex.Match(leftHand.name, ice_cubeRegex).Success) {
        //             ui.HoldIce_Cube(controllingHand);
        //         }
        //     }
        //     else if(controllingHand == hand.Right)
        //     {
        //         if(Regex.Match(rightHand.name, appleRegex).Success){
        //             ui.HoldApple(controllingHand);
        //         }
        //         else if(Regex.Match(rightHand.name, ice_cubeRegex).Success) {
        //             ui.HoldIce_Cube(controllingHand);
        //         }
        //     }
        // }

       
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
        if (!lookingAtObject)
        {
            //Debug.Log("Pressed left click (pick up), not looking at/close enough to object");
            return; // ha ha
        }
        
        if(controllingHand == hand.Left)
        {
            if(amountOfItemsHeldLeft < currentInventoryCapacityLeft)
            {
                Transform t = cam.GetComponent<Transform>();
                Vector3 pos = t.position;
                Vector3 dir = t.TransformDirection(Vector3.forward);
                RaycastHit hit;

                // origin, direction, where to put the raycast, distance to cast, layer
                bool lookingAtObject = Physics.Raycast(pos, dir, out hit, 20, itemLayerMask);
                //Debug.DrawRay(pos, dir, Color.red, 10);

                amountOfItemsHeldLeft++;
                if (amountOfItemsHeldLeft > 1)
                {
                    leftHandInventory.Push(leftHand);
                }

                leftHand = hit.collider.gameObject; // set the object being held
                                                    // Destroy(hit.collider.gameObject);
                leftHand.SetActive(false);
                Debug.Log("We picked up " + leftHand.name);
            }
            else
            {
                Debug.Log("Can't pick up anymore items!");
            }
        }
        else if(controllingHand == hand.Right)
        {
            if(amountOfItemsHeldRight < currentInventoryCapacityRight)
            {
                amountOfItemsHeldRight++;
                if (amountOfItemsHeldRight > 1)
                {
                    rightHandInventory.Push(rightHand);
                }

                rightHand = hit.collider.gameObject; // set the object being held
                                                    // Destroy(hit.collider.gameObject);
                rightHand.SetActive(false);
                Debug.Log("We picked up " + rightHand.name);
            }
            else
            {
                Debug.Log("Can't pick up anymore items!");
            }
        }
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
        if(controllingHand == hand.Left)
        {
            if(amountOfItemsHeldLeft > 0)
            {
                Transform t1 = cam.GetComponent<Transform>();
                leftHand.SetActive(true);
                leftHand.transform.position = new Vector3(t1.position.x, t1.position.y, t1.position.z) + transform.rotation * Vector3.forward;
                Debug.Log("We dropped " + leftHand.name);

                if (amountOfItemsHeldLeft == 1)
                {
                    leftHand = null;
                    ui.Idle(controllingHand);
                    amountOfItemsHeldLeft--;
                }
                else if (amountOfItemsHeldLeft > 1)
                {
                    leftHand = (GameObject)leftHandInventory.Pop();
                    amountOfItemsHeldLeft--;
                }
            }
        }
        else if(controllingHand == hand.Right)
        {
            if(amountOfItemsHeldRight > 0)
            {
                Transform t1 = cam.GetComponent<Transform>();
                rightHand.SetActive(true);
                rightHand.transform.position = new Vector3(t1.position.x, t1.position.y, t1.position.z) + transform.rotation * Vector3.forward;
                Debug.Log("We dropped " + rightHand.name);

                if (amountOfItemsHeldRight == 1)
                {
                    rightHand = null;
                    ui.Idle(controllingHand);
                    amountOfItemsHeldRight--;
                }
                else if (amountOfItemsHeldRight > 1)
                {
                    rightHand = (GameObject)rightHandInventory.Pop();
                    amountOfItemsHeldRight--;
                }
            }
        }
    }

    private void SwitchHand() {
        if(controllingHand == hand.Left)
        {
            controllingHand = hand.Right;
            Debug.Log("now controlling right hand");
        }
        else if(controllingHand == hand.Right) 
        { 
            controllingHand = hand.Left;
            Debug.Log("now controlling left hand");
        }
    }
}

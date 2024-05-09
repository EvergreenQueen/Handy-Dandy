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
    [SerializeField] float camDegLimit = 90.0f;
    [SerializeField] float roomForError = 1.0f;
    [SerializeField] float timeScale = 1.0f;
    [SerializeField] float sprintMult = 1.0f;

    //Objects
    PlayerActionControls pc;
    Rigidbody rb;
    Camera cam;
    [SerializeField] UIManager ui;

    Transform t;
    Vector3 pos;
    Vector3 dir;

    bool lookingAtObject = false;    
    bool lookingAtNPC = false;
    RaycastHit hit;
    int itemLayerMask = 1 << 7; // huh.
    int NPCLayerMask = 1 << 8; //npc npc npc
    GameObject leftHand, rightHand; // what either hand is carrying
    //Variables
    LayerMask isGround;
    float currentCamRotation = 0.0f;



    void Awake()
    {   
        
        pc = new PlayerActionControls();
        pc.Movement.Enable();
        pc.Movement.WASD.Enable();
        pc.Movement.LookAround.Enable();
        pc.Movement.E.Enable(); //enabling E for character interaction?
        // pc.Movement.Click.Enable();
        // pc.Movement.Click.performed += _ => PickUp();
        pc.Movement.Sprint.performed += _ => Sprint(true);
        pc.Movement.Sprint.canceled += _ => Sprint(false);


        pc.Movement.LClick.Enable();
        pc.Movement.RClick.Enable();

        pc.Movement.LClick.performed += _ => PickUp();
        pc.Movement.RClick.performed += _ => Drop();

        rb = gameObject.GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();

        // isGround = LayerMask.NameToLayer("Ground");
        isGround = 1 <<  LayerMask.NameToLayer("Ground");
        Debug.Log(LayerMask.NameToLayer("Ground"));
        // isGround;

        leftHand = rightHand = null;
    }


    // Update is called once per frame
    void Update()
    {
        t = cam.GetComponent<Transform>();
        pos = t.position;
        dir = t.TransformDirection(Vector3.forward);

        // origin, direction, where to put the raycast, distance to cast, layer
        lookingAtObject = Physics.Raycast(pos, dir, out hit, 1000, itemLayerMask);
        //Debug.DrawRay(pos, dir, Color.red, 10);

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

        lookingAtNPC = Physics.Raycast(pos, dir, out hit, 1000, NPCLayerMask);

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

    private void OnTriggerStay(Collider target) {

        //getting ready to Interact with the NPC
        if(target.tag == "NPC") {
            if (lookingAtNPC) {
                Debug.DrawRay(pos, dir, Color.blue, 10);
            }
        }
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
        Vector2 rawInput = pc.Movement.LookAround.ReadValue<Vector2>(); //Moving up is pos, down is negative
        Debug.Log(rawInput);
        float verticalChange = -rawInput.y * camSensY;

        //Translating x direction to PLAYER y rotation
        transform.Rotate(0.0f, rawInput.x * camSensX, 0.0f, Space.Self);

        //Translating y direction to camera x rotation
        currentCamRotation += verticalChange;
        Debug.Log("currentRotation: " + currentCamRotation);
        if(currentCamRotation > camDegLimit + roomForError){
            Debug.Log("too high");
            verticalChange = camDegLimit - (currentCamRotation-verticalChange);
            cam.transform.Rotate(verticalChange, 0.0f, 0.0f, Space.Self);
            currentCamRotation = camDegLimit;

        }else if(currentCamRotation < -camDegLimit - roomForError){
            Debug.Log("too low");
            verticalChange = -camDegLimit - (currentCamRotation-verticalChange);
            cam.transform.Rotate(verticalChange, 0.0f, 0.0f, Space.Self); 
            currentCamRotation = -camDegLimit;
        }else{
            cam.transform.Rotate(verticalChange, 0.0f, 0.0f, Space.Self);    
        }
        


    }

    private void PickUp() {
        
        if(!lookingAtObject)
        {
            Debug.Log("Pressed left click (pick up), not looking at/close enough to object");
            return; // ha ha
        } 
        leftHand = hit.collider.gameObject; // set the object being held
        // Destroy(hit.collider.gameObject);
        leftHand.SetActive(false);
        Debug.Log("We picked up " + leftHand.name);

        // pick up an item, i guess???

    }

    //come on and interact with that hot NPC with E
    private void InteractionWithNPC() {

    }

    void Sprint(bool b){
        if(b){
            speed *= sprintMult;
        }else{
            speed = speed/sprintMult;
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

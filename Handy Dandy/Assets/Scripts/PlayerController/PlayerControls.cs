using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;
using Yarn.Unity;

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
    int NPCLayerMask = 1 << 8; //npc npc npc
    Transform t;
    Vector3 pos;
    Vector3 dir;

    public enum containerType { Hand, Basket };
    public enum whichContainer { Left, Right };
    whichContainer controllingContainer = whichContainer.Left;
    containerType[] containers = { containerType.Hand, containerType.Hand };
    // left container       right container
    GameObject leftHand, rightHand; // what either hand is carrying
    //Variables
    LayerMask isGround;
    float currentCamRotation = 0.0f;
    bool isGrounded = false;
    bool soundIsPlaying = false;
    float slopeAngle;
    float playerheight;
    RaycastHit slopeHit;
    public Stack rightHandInventory = new Stack(inventorySize);
    public Stack leftHandInventory = new Stack(inventorySize);
    int currentInventoryCapacityLeft = 5, currentInventoryCapacityRight = 5;
    int amountOfItemsHeldLeft = 0, amountOfItemsHeldRight = 0;
    AudioSource audioSource;
    AudioManager audioManager;
    string appleRegex = @"Apple.*", ice_cubeRegex = @"Ice_Cube.*", mouseRegex = @"Mouse.*", catRegex = @"Cat.*";
    public DialogueRunner dialogueRunner;
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
        pc.Movement.E.performed += _ => InteractWithNPC();


        pc.Movement.LClick.Enable();
        pc.Movement.RClick.Enable();

        pc.Movement.LClick.performed += _ => PickUp();
        pc.Movement.RClick.performed += _ => Drop();
        pc.Movement.SwitchHand.performed += _ => SwitchHand();

        rb = gameObject.GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerheight = capsuleCollider.height;
        audioSource = GetComponentInChildren<AudioSource>();
        audioManager = GetComponentInChildren<AudioManager>();
        audioSource.clip = audioManager.sounds[0];
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        isGround = 1 << LayerMask.NameToLayer("Ground");

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
        t = cam.GetComponent<Transform>();
        pos = t.position;
        dir = t.TransformDirection(Vector3.forward);

        // origin, direction, where to put the raycast, distance to cast, layer
        lookingAtObject = Physics.Raycast(pos, dir, out hit, 1000, itemLayerMask);
        Debug.DrawRay(pos, dir, Color.red, 10);

        visualUpdateContainers();
        {
            // if(leftHand == null) { ui.Idle(whichContainer.Left); ui.Drop(whichContainer.Left); }
            // if(rightHand == null) { ui.Idle(whichContainer.Right); ui.Drop(whichContainer.Right); }
            // if((controllingContainer == whichContainer.Left && leftHand == null) || (controllingContainer == whichContainer.Right && rightHand == null)) // long code lmfao
            // {
            //     // if the controlling hand is empty
            //     if(lookingAtObject) {
            //         ui.Point(controllingContainer);
            //     }
            //     else {
            //         ui.Idle(controllingContainer);
            //     }
            // }
            // else
            // {
            //     ItemIdentification item = null; // im gonna eat my hands
            //     if(controllingContainer == whichContainer.Left)
            //     {
            //         item = leftHand.GetComponent<ItemIdentification>();
            //     }
            //     else if(controllingContainer == whichContainer.Right)
            //     {
            //         item = rightHand.GetComponent<ItemIdentification>();
            //     }

            //     if(item.containsTag(ItemIdentification.ListOfPossibleTags.Animal))
            //     {
            //         ui.Grip_Loose(controllingContainer);
            //     }
            //     else
            //     {
            //         ui.Hold(controllingContainer);
            //     }

            //     string itemName = "";
            //     if(controllingContainer == whichContainer.Left)
            //     {
            //         itemName = leftHand.name;
            //     }
            //     else if(controllingContainer == whichContainer.Right)
            //     {
            //         itemName = rightHand.name;
            //     }

            //     if(Regex.Match(itemName, appleRegex).Success){
            //         ui.HoldApple(controllingContainer);
            //     }
            //     else if(Regex.Match(itemName, ice_cubeRegex).Success) {
            //         ui.HoldIce_Cube(controllingContainer);
            //     }
            //     else if(Regex.Match(itemName, mouseRegex).Success) {
            //         ui.HoldMouse(controllingContainer);
            //     }
            //     else if(Regex.Match(itemName, catRegex).Success) {
            //         ui.HoldCat(controllingContainer);
            //     }
            // } //
        }

        // if(lookingAtObject && leftHand == null) {
        //     ui.Point();
        // }
        // else if(leftHand == null)
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

    // VISUAL UPDATE ONLY. NO HOLDING/DROPPING LOGIC
    void visualUpdateContainers()
    {
        // iterate over all containers (here, 2 times. left and right)
        for (int which = (int)whichContainer.Left; which <= (int)whichContainer.Right; which++)
        {
            containerType type = containers[which];             // 0 = hand, 1 = basket...
            whichContainer whichCont = (whichContainer)which;   // 0 = left, 1 = right
            GameObject itemToDisplay = null;
            ItemIdentification itemInfo = null;

            // not a huge fan of this implementation, but our display is weird anyway
            if (whichCont == whichContainer.Left) itemToDisplay = leftHand; // left
            else if (whichCont == whichContainer.Right) itemToDisplay = rightHand; // right

            // important for tags
            if (itemToDisplay != null) itemInfo = itemToDisplay.GetComponent<ItemIdentification>();

            switch (type)
            {
                case containerType.Hand:
                    // stupid complex logic here
                    if (itemToDisplay == null)
                    {
                        // if looking at smth, point, else idle
                        if (lookingAtObject && controllingContainer == whichCont) ui.ChangeHands(whichCont, UIManager.State.Point);
                        else ui.ChangeHands(whichCont, UIManager.State.Idle);
                    }
                    else
                    {
                        // if holding grippable, grip, else hold
                        if (itemInfo.containsTag(ItemIdentification.ListOfPossibleTags.Grippable)) ui.ChangeHands(whichCont, UIManager.State.Grip_Loose);
                        else ui.ChangeHands(whichCont, UIManager.State.Hold);
                    }

                    break;
                case containerType.Basket:
                    // just draw the basket
                    ui.ChangeHands(whichCont, UIManager.State.Basket);
                    break;
                default:
                    break;
            }

            // item display
            if (itemToDisplay == null) ui.Drop(whichCont);
            else
            {
                int itemId = itemInfo.id;
                ui.HoldItem(whichCont, (UIManager.Item)itemId);
            }
        }
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
            if (!input.Equals(Vector3.zero) && !soundIsPlaying)
            {
                audioSource.Play();
                soundIsPlaying = true;
            }
        }
        else if (soundIsPlaying)
        {
            soundIsPlaying = false; audioSource.Stop();
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
        
        if(controllingContainer == whichContainer.Left)
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
                leftHand = hit.collider.gameObject; // set the object being held
                leftHandInventory.Push(leftHand);

                leftHand.SetActive(false);
                Debug.Log("We picked up " + leftHand.name);
                Debug.Log("lhs count: " + leftHandInventory.Count);
            }
            else
            {
                Debug.Log("Can't pick up anymore items!");
            }
        }
        else if(controllingContainer == whichContainer.Right)
        {
            if(amountOfItemsHeldRight < currentInventoryCapacityRight)
            {
                Transform t = cam.GetComponent<Transform>();
                Vector3 pos = t.position;
                Vector3 dir = t.TransformDirection(Vector3.forward);
                RaycastHit hit;

                // origin, direction, where to put the raycast, distance to cast, layer
                bool lookingAtObject = Physics.Raycast(pos, dir, out hit, 20, itemLayerMask);
                amountOfItemsHeldRight++;

                rightHand = hit.collider.gameObject; // set the object being held
                rightHandInventory.Push(rightHand);
                                                    // Destroy(hit.collider.gameObject);
                rightHand.SetActive(false);
                Debug.Log("We picked up " + rightHand.name);
                Debug.Log("rhs count: " + rightHandInventory.Count);
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
            audioSource.clip = audioManager.sounds[1];
            audioSource.Play();
        }
        else{
            speed = speed/sprintMult;
            audioSource.clip = audioManager.sounds[0];
            audioSource.Play();
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private void Drop()
    {
        if (controllingContainer == whichContainer.Left)
        {
            if (amountOfItemsHeldLeft > 0)
            {
                Transform t1 = cam.GetComponent<Transform>();
                leftHand.SetActive(true);
                leftHand.transform.position = new Vector3(t1.position.x, t1.position.y, t1.position.z) + transform.rotation * Vector3.forward;
                Debug.Log("We dropped " + leftHand.name);

                leftHandInventory.Pop();
                if (amountOfItemsHeldLeft == 1)
                {
                    leftHand = null;
                    ui.Idle(controllingContainer);
                }
                else if (amountOfItemsHeldLeft > 1)
                {
                    leftHand = (GameObject)leftHandInventory.Peek();
                }
                amountOfItemsHeldLeft--;
                Debug.Log("lhs count: " + leftHandInventory.Count);
            }
        }
        else if (controllingContainer == whichContainer.Right)
        {
            if (amountOfItemsHeldRight > 0)
            {
                Transform t1 = cam.GetComponent<Transform>();
                rightHand.SetActive(true);
                rightHand.transform.position = new Vector3(t1.position.x, t1.position.y, t1.position.z) + transform.rotation * Vector3.forward;
                Debug.Log("We dropped " + rightHand.name);

                rightHandInventory.Pop();
                if (amountOfItemsHeldRight == 1)
                {
                    rightHand = null;
                    ui.Idle(controllingContainer);
                }
                else if (amountOfItemsHeldRight > 1)
                {
                    rightHand = (GameObject)rightHandInventory.Peek();
                }
                amountOfItemsHeldRight--;
                Debug.Log("rhs count: " + rightHandInventory.Count);
            }
        }
    }

    private void SwitchHand() {
        if(controllingContainer == whichContainer.Left)
        {
            controllingContainer = whichContainer.Right;
            Debug.Log("now controlling right hand");
        }
        else if(controllingContainer == whichContainer.Right) 
        { 
            controllingContainer = whichContainer.Left;
            Debug.Log("now controlling left hand");
        }
    }

    private void InteractWithNPC(){
        if(!(Physics.Raycast(pos, dir, out hit, 3, NPCLayerMask))){
            return;
        }else{
            Debug.Log(hit.collider.gameObject.name);
            dialogueRunner.StartDialogue("TieGuyDialogueIntro");
        }
    }
}

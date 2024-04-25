using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float speed = 1.0f;
    [SerializeField] float grav = -9.81f;
    [SerializeField] float floatingDistance = 1.0f;
    

    //Objects
    PlayerActionControls pc;
    Rigidbody rb;

    //Variables
    LayerMask isGround;


    void Awake()
    {   
        // Debug.Log("startaawakes");
        pc = new PlayerActionControls();
        pc.Movement.WASD.Enable();
        rb = gameObject.GetComponent<Rigidbody>();

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
        
        checkGroundDist();
    }

    void FixedUpdate()
    {
        //For gravity:

        Vector3 gravity = new Vector3(0, grav, 0);
        Move();
        // Debug.Log("what");
        rb.AddForce(gravity);
        
    }

    private void Move()
    {  
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

//      Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 100, Color.yellow);


        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, isGround))
        {
            // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            
            // if(hit.distance - floatingDistance < 0) transform.position += new Vector3(0.0f, floatingDistance - hit.distance, 0.0f);
            transform.position += new Vector3(0.0f, floatingDistance - hit.distance, 0.0f);

        }


    }
}

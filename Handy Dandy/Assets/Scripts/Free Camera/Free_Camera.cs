using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
/* -------------------------------------------------
 * CONTROLS: 
 * 
 * 1) wasd for movement
 * 2) mouse for orientation
 * 3) tab to lock orientation
 * -------------------------------------------------
 */
public class Free_Camera : MonoBehaviour
{
    [SerializeField] float default_speed = 5f;
    [SerializeField] float camera_sensitivity = 300f;
    [SerializeField] GameObject player;
    float movement_speed;
    float vertical_Rotation;
    float horizontal_Rotation;

    float mouseX;
    float mouseY;

    float X_Input;
    float Y_Input;
    float Z_Input;

    // Lock Camera on start to not conflict with player
    bool cameraLock = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        vertical_Rotation = transform.rotation.y;
        horizontal_Rotation = transform.rotation.x;
        movement_speed = default_speed; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!cameraLock)
        {
            // Camera orientation input
            mouseX = Input.GetAxis("Mouse X") * camera_sensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * camera_sensitivity * Time.deltaTime;

            // Camera movement input
            Y_Input = Input.GetAxisRaw("Vertical");
            X_Input = Input.GetAxisRaw("Horizontal");

            horizontal_Rotation += mouseX;
            vertical_Rotation -= mouseY;

            if (Input.GetKey("space"))
            {
                Z_Input = 1;
            }
            else if (Input.GetKey("left ctrl"))
            {
                Z_Input = -1;
            }
            else
            {
                Z_Input = 0;
            }

            if (Input.GetKey("left shift"))
            {
                movement_speed = default_speed * 2;
            }
            else
            {
                movement_speed = default_speed;
            }

            // camera orientation
            vertical_Rotation = Mathf.Clamp(vertical_Rotation, -90f, 90f);
            transform.rotation = Quaternion.Euler(vertical_Rotation, horizontal_Rotation, 0);
        }
        if (Input.GetKeyDown("tab"))
        {
            cameraLock = !cameraLock;
            player.gameObject.SetActive(cameraLock);
        }
    }

    void FixedUpdate()
    {
        // god mode camera movement
        Vector3 movementDirection = new Vector3(X_Input, Z_Input, Y_Input);
        transform.Translate(movementDirection * movement_speed * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] Transform target_camera;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 direction = target_camera.position - transform.position;

        transform.LookAt(transform.position + new Vector3(direction.x, 0, direction.z));
    }
}

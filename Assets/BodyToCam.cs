using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyToCam : MonoBehaviour
{

    [SerializeField] private Transform cameraObject;

    [SerializeField] private Vector3 BodyOffset, rotationOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraObject.position + BodyOffset;
        transform.forward = Vector3.ProjectOnPlane(cameraObject.up, Vector3.up).normalized;
        transform.rotation *= Quaternion.Euler(rotationOffset);
    }
}

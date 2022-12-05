using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeadFollow : MonoBehaviour
{

    [SerializeField] private Transform _RootObject, _FollowObject;

    [SerializeField] private Vector3 _PositionOffset, _RotationOffset;
    [SerializeField] private Vector3 _headBodyOffset;




    public InputAction resetBody;

    // Start is called before the first frame update


    // Update is called once per frameU

    private void Update()
    {
        if (resetBody.triggered) 
        {
            _RootObject.position = _FollowObject.position + _headBodyOffset;
            _RootObject.forward = Vector3.ProjectOnPlane(_FollowObject.up, Vector3.up).normalized;
        }
    }
    void LateUpdate()
    {
        //RootObject.position = transform.position + _headBodyOffset;    
        //_RootObject.forward = Vector3.ProjectOnPlane(_FollowObject.up, Vector3.up).normalized;

        transform.position = _FollowObject.position;
        transform.rotation = _FollowObject.rotation * Quaternion.Euler(_RotationOffset);
    }

    private void Awake()
    {
        _RootObject.position = _FollowObject.position + _headBodyOffset;    
        _RootObject.forward = Vector3.ProjectOnPlane(_FollowObject.up, Vector3.up).normalized;
        
    }
    private void Start()
    {
        resetBody.Enable();
    }
}

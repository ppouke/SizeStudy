using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadFollow : MonoBehaviour
{

    [SerializeField] private Transform _RootObject, _FollowObject;

    [SerializeField] private Vector3 _PositionOffset, _RotationOffset, _headBodyOffset;

    
    // Start is called before the first frame update
    

    // Update is called once per frame
    void LateUpdate()
    {
        _RootObject.position = transform.position + _headBodyOffset;
        _RootObject.forward = Vector3.ProjectOnPlane(_FollowObject.up, Vector3.up).normalized;

        transform.position = _FollowObject.TransformPoint(_PositionOffset);
        transform.rotation = _FollowObject.rotation * Quaternion.Euler(_RotationOffset);
    }
}

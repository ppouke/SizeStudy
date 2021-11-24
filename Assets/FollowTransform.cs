using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{

    [SerializeField] private Transform followTarget;
    [SerializeField] private Vector3 rotationOffset;

    // Start is called before the first frame update
    void Start()
    {

        gameObject.transform.position = followTarget.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {

        gameObject.transform.position = followTarget.transform.position;
        gameObject.transform.rotation = followTarget.transform.rotation * Quaternion.Euler(rotationOffset);

    }
}

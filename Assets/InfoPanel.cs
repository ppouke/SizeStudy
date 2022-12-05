using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InfoPanel : MonoBehaviour
{

    [SerializeField] private Transform lookAtObject;
    [SerializeField] private GameObject MasterScriptObject;

    public Vector3 rotOffset = new Vector3(0, -90, 90);

    public InputAction scale;
    public InputAction height;
    public InputAction location;
    public InputAction body;

    public InputAction hideSheet;

    bool hidden;

    private MasterController master;
    private GameObject infoSheet;
    // Start is called before the first frame update
    void Start()
    {
        scale.Enable();
        height.Enable();
        location.Enable();
        body.Enable();
        hideSheet.Enable();
        master = MasterScriptObject.GetComponent<MasterController>();
        infoSheet = transform.GetChild(0).gameObject;
        hidden = false;
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(new Vector3(lookAtObject.position.x, transform.position.y, lookAtObject.position.z));

        if (scale.triggered) 
        {
            if(master.scaleIndex == 2) 
            {
                master.changeScale(-2);
            }
            else 
            {
                master.changeScale(1);
            }
        }

        if (height.triggered)
        {

            if (master.heightIndex == 2)
            {
                master.changeHeight(-2);
            }
            else
            {
                master.changeHeight(1);
            }
        }
        if (location.triggered)
        {
            if (master.locIndex == 2)
            {
                master.changeLocation(-2);
            }
            else
            {
                master.changeLocation(1);
            }
        }
        if (body.triggered)
        {
            if (master.bodyIndex == 1)
            {
                master.changeBody(-1);
            }
            else
            {
                master.changeBody(1);
            }
        }

        if (hideSheet.triggered) 
        {
            Debug.Log("hide Sheet");
            hidden = !hidden;
            infoSheet.SetActive(!hidden);
        }
    }
}

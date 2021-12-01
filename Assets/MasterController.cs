using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.XR;

public class MasterController : MonoBehaviour
{

    private Vector3[] scaleChanges = new Vector3[3];

    


    public float[] scaleValues = new float[3];
    public float[] heightValues = new float[3];

    public Transform[] locations = new Transform[3];

    public InputAction upDownModifier;

    public InputAction changeScaleAction;
    public InputAction changeHeightAction;
    public InputAction changeLocationAction;
    public InputAction changeBodyAction;

    public InputAction concealLoc;



    [SerializeField] private GameObject iKBody;
    [SerializeField] private GameObject[] hands = new GameObject[2];

    [SerializeField] private GameObject hiderObj;

    private int bodyIndex;
    private int scaleIndex;
    private int heightIndex;
    private int locIndex;

    private bool isHidden;

    public TMP_Text text;
    
    

    // Start is called before the first frame update
    void Start()
    {

       

        for(int i = 0; i < scaleValues.Length; i++) 
        {
            scaleChanges[i] = new Vector3(1, 1, 1) * scaleValues[i];
        }
        
        

        changeScaleAction.Enable();
        changeHeightAction.Enable();
        changeLocationAction.Enable();
        changeBodyAction.Enable();
        upDownModifier.Enable();
        concealLoc.Enable();
        scaleIndex = 0;
        heightIndex = 0;
        locIndex = 0;
        bodyIndex = 0;

        //set rig init position
        Vector3 locPos = locations[0].position;
        gameObject.transform.position = new Vector3(locPos.x, heightValues[0], locPos.z);

        //set init body
        for (int i = 0; i < hands.Length; i++)
        {
            hands[i].SetActive(false);
        }
        iKBody.SetActive(false);

        //set hidden
        isHidden = true;

        hiderObj.SetActive(true);
        

        XRSettings.enabled = false;
        string currConfig = $"S,H,L,B \n{scaleIndex},{heightIndex}, {locIndex}, {bodyIndex}";
        text.SetText(currConfig);
    }

    // Update is called once per frame
    void Update()
    {

        int upDownFloat = (int)upDownModifier.ReadValue<float>();

        if(changeScaleAction.triggered)
        {

            scaleIndex = Mathf.Max(Mathf.Min(scaleIndex + upDownFloat, 2),0);
            string currConfig = $"S,H,L,B \n{scaleIndex},{heightIndex}, {locIndex}, {bodyIndex}";
            text.SetText(currConfig);

            gameObject.transform.localScale = scaleChanges[scaleIndex];
            hiderObj.transform.localScale = new Vector3(
                1/scaleChanges[scaleIndex].x,
                1/scaleChanges[scaleIndex].y,
                1/scaleChanges[scaleIndex].z) * 0.1f;

           
  
        }

        if (changeHeightAction.triggered) 
        {
            heightIndex = Mathf.Max(Mathf.Min(heightIndex + upDownFloat, 2),0);
            string currConfig = $"S,H,L,B \n{scaleIndex},{heightIndex}, {locIndex}, {bodyIndex}";
            text.SetText(currConfig);

            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, heightValues[heightIndex], gameObject.transform.localPosition.z);
        }

        if (changeLocationAction.triggered) 
        {
            locIndex = Mathf.Max(Mathf.Min(locIndex + upDownFloat, 2),0);
            string currConfig = $"S,H,L,B \n{scaleIndex},{heightIndex}, {locIndex}, {bodyIndex}";
            text.SetText(currConfig);

            Vector3 locPos = locations[locIndex].position;
            gameObject.transform.position = new Vector3(locPos.x, heightValues[heightIndex], locPos.z);
        }

        if (changeBodyAction.triggered) 
        {
            bodyIndex = Mathf.Max(Mathf.Min(bodyIndex + upDownFloat, 1),0);
            string currConfig = $"S,H,L,B \n{scaleIndex},{heightIndex}, {locIndex}, {bodyIndex}";
            text.SetText(currConfig);
            //cant get ik sys to work 

            if (bodyIndex == 1) 
            {
                for(int i = 0; i < hands.Length; i++) 
                {
                    hands[i].SetActive(true);
                }
                iKBody.SetActive(false);
            }

            if(bodyIndex == 2) 
            {
                for (int i = 0; i < hands.Length; i++)
                {
                    hands[i].SetActive(false);

                }
                iKBody.SetActive(true);

            }

            if (bodyIndex == 0) {
                for (int i = 0; i < hands.Length; i++)
                {
                    hands[i].SetActive(false);
                }
                iKBody.SetActive(false);
            }

            
        }

        if (concealLoc.triggered) 
        {
            isHidden = !isHidden;

            if (isHidden) 
            {
                hiderObj.SetActive(true);
                XRSettings.enabled = false;
                string currConfig = $"S,H,L,B \n{scaleIndex},{heightIndex}, {locIndex}, {bodyIndex}";
                text.SetText(currConfig);
                

            }
            else 
            {
                
                hiderObj.SetActive(false);
                XRSettings.enabled = true;

            }
        }

        
    }
}

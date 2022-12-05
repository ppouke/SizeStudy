using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.XR;
using System.IO;
using System;

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

    public InputAction showPanel;

    public InputAction concealLoc;

    public InputAction recordChanges;


    
    [SerializeField] private GameObject iKBody;
    [SerializeField] private GameObject[] hands = new GameObject[2];
    [SerializeField] private Transform[] controllers = new Transform[2];
    [SerializeField] private Vector3 rotationOffset;

    [SerializeField] private Transform handOrigin;
   
    [SerializeField] private float[] handTrackingDistances = new float[3];
    [SerializeField] private GameObject hiderObj;

    [SerializeField] private GameObject jacketOffset;
    [SerializeField] private float[] heightOffsets = new float[3];
    [SerializeField] private GameObject[] panelItems = new GameObject[3];
    public int bodyIndex;
    public int scaleIndex;
    public int heightIndex;
    public int locIndex;

    private int curTrial;
    private bool recording;
    
    private bool panelShowing;
    private string docName;

    private bool isHidden;

    public TMP_Text text;
    public GameObject recText;
    private TMP_Text recNum;
    
    

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
        recordChanges.Enable();
        showPanel.Enable();
        scaleIndex = 0;
        heightIndex = 0;
        locIndex = 0;
        bodyIndex = 0;

        //set rig init position
        Vector3 locPos = locations[0].position;
        gameObject.transform.position = new Vector3(locPos.x, heightValues[0], locPos.z);

        jacketOffset.transform.localPosition = new Vector3(
            jacketOffset.transform.localPosition.x,
            heightOffsets[0],
            jacketOffset.transform.localPosition.z);



        //set init body
        for (int i = 0; i < hands.Length; i++)
        {
            hands[i].SetActive(false);
        }
        iKBody.SetActive(false);
      



        //set hidden
        isHidden = true;
        hiderObj.SetActive(true);
        panelShowing = false;
        for (int i = 0; i < panelItems.Length; i++)
        {
            panelItems[i].SetActive(panelShowing);
        }



        string currConfig = $"S,H,L,B \n{scaleIndex},{heightIndex},{locIndex},{bodyIndex}";
        text.SetText(currConfig);

        
        //  Set Recording from new
        curTrial = 0;
        recording = false;
        recText.SetActive(false);
        recNum = recText.GetComponent<TMP_Text>();

         //create directory
         if(!Directory.Exists(Application.streamingAssetsPath + "/Trial_Logs/"))
         {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Trial_Logs/");
         }
        
        //count files and append new
        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath + "/Trial_Logs/");
        FileInfo[] info = dir.GetFiles("*.txt");
        int fileCount = info.Length;
        docName = Application.streamingAssetsPath + "/Trial_Logs/" + $"Participant {fileCount + 1} " + ".txt";
       

    }

    // Update is called once per frame
    void Update()
    {

        //Hand tracking stuff


        for(int i = 0; i < hands.Length; i++) 
        {
            Vector3 controllerDiff = controllers[i].transform.position - handOrigin.position;

            //Vector3 extraOffset = Vector3.Lerp(handOriginOffset[0], handOrigin.position, scaleIndex);
            hands[i].transform.position = handOrigin.position + (controllerDiff * handTrackingDistances[scaleIndex]);
            //hands[i].transform.position = controllers[i].transform.position;

            

            hands[i].transform.rotation = controllers[i].transform.rotation * Quaternion.Euler(rotationOffset);
            rotationOffset = -rotationOffset;
            //handOriginOffset = new Vector3(-handOriginOffset.x, handOriginOffset.y, handOriginOffset.z);
        }

        if (showPanel.triggered)
        {
            panelShowing = !panelShowing;
            for(int i = 0; i < panelItems.Length; i++) 
            {
                panelItems[i].SetActive(panelShowing);
            }
            
            
        }


        //Recording stuff

        if (recordChanges.triggered) 
        {
            recording = !recording;
            
            recText.SetActive(recording);
            if (recording) 
            {
                curTrial += 1;
                recNum.SetText($"Rec {curTrial}");
                File.AppendAllText(docName, $"\n________________________\nTrial {curTrial}:\nStaring conf: {scaleIndex},{heightIndex},{locIndex},{bodyIndex}\n");
            }
            else 
            {
                File.AppendAllText(docName, $"\nFinal conf: {scaleIndex},{heightIndex},{locIndex},{bodyIndex}");
            }
            
        }

        //changes stuff
        int upDownFloat = (int)upDownModifier.ReadValue<float>();

        if(changeScaleAction.triggered)
        {

            changeScale(upDownFloat);
            
        }

        if (changeHeightAction.triggered) 
        {
            changeHeight(upDownFloat);
        }

        if (changeLocationAction.triggered) 
        {
            changeLocation(upDownFloat);
        }

        if (changeBodyAction.triggered) 
        {
            changeBody(upDownFloat);

        }

        if (concealLoc.triggered) 
        {
            isHidden = !isHidden;

            if (isHidden) 
            {
                hiderObj.SetActive(true);
               
                string currConfig = $"S,H,L,B \n{scaleIndex},{heightIndex}, {locIndex}, {bodyIndex}";
                text.SetText(currConfig);
                

            }
            else 
            {
                
                hiderObj.SetActive(false);
                

            }
        }



        
    }

    public void changeBody(int dirModifier)
    {
        bodyIndex = Mathf.Max(Mathf.Min(bodyIndex + dirModifier, 1), 0);
        string currConfig = $"S,H,L,B \n{scaleIndex},{heightIndex}, {locIndex}, {bodyIndex}";
        text.SetText(currConfig);
        //cant get ik sys to work 

        if (bodyIndex == 1)
        {
            for (int i = 0; i < hands.Length; i++)
            {
                hands[i].SetActive(true);
            }
            iKBody.SetActive(false);


        }

        if (bodyIndex == 2)
        {
            for (int i = 0; i < hands.Length; i++)
            {
                hands[i].SetActive(false);

            }
            iKBody.SetActive(true);

        }

        if (bodyIndex == 0)
        {
            for (int i = 0; i < hands.Length; i++)
            {
                hands[i].SetActive(false);
            }
            iKBody.SetActive(false);
        }


        if (recording)
        {
            File.AppendAllText(docName, "B ");
        }
    }

    public void changeLocation(int dirModifier)
    {
        locIndex = Mathf.Max(Mathf.Min(locIndex + dirModifier, 2), 0);
        string currConfig = $"S,H,L,B \n{scaleIndex},{heightIndex}, {locIndex}, {bodyIndex}";
        text.SetText(currConfig);

        Vector3 locPos = locations[locIndex].position;
        gameObject.transform.position = new Vector3(locPos.x, heightValues[heightIndex], locPos.z);

        if (recording)
        {
            File.AppendAllText(docName, "L ");
        }
    }

    public void changeScale(int dirModifier) 
    {
        scaleIndex = Mathf.Max(Mathf.Min(scaleIndex + dirModifier, 2), 0);
        string currConfig = $"S,H,L,B \n{scaleIndex},{heightIndex}, {locIndex}, {bodyIndex}";
        text.SetText(currConfig);

        gameObject.transform.localScale = scaleChanges[scaleIndex];

        //change hider cube scale
        hiderObj.transform.localScale = new Vector3(
            1 / scaleChanges[scaleIndex].x,
            1 / scaleChanges[scaleIndex].y,
            1 / scaleChanges[scaleIndex].z) * 0.1f;

        //change jacket y offset
        jacketOffset.transform.localPosition = new Vector3(
        jacketOffset.transform.localPosition.x,
        heightOffsets[scaleIndex],
        jacketOffset.transform.localPosition.z);



        if (recording)
        {
            File.AppendAllText(docName, "S ");
        }
    }

    public void changeHeight(int dirModifier) 
    {
        heightIndex = Mathf.Max(Mathf.Min(heightIndex + dirModifier, 2), 0);
        string currConfig = $"S,H,L,B \n{scaleIndex},{heightIndex}, {locIndex}, {bodyIndex}";
        text.SetText(currConfig);

        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, heightValues[heightIndex], gameObject.transform.localPosition.z);






        if (recording)
        {
            File.AppendAllText(docName, "H ");
        }
    }

    
}

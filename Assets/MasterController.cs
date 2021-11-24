using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MasterController : MonoBehaviour
{

    private Vector3[] scaleChanges = new Vector3[3];

    


    public float[] scaleValues = new float[3];
    public float[] heightValues = new float[3];

    public Transform[] locations = new Transform[3];

    public InputAction changeScaleAction;
    public InputAction changeHeightAction;
    public InputAction changeLocationAction;
    public InputAction changeBodyAction;


    [SerializeField] private GameObject iKBody;
    [SerializeField] private GameObject[] hands = new GameObject[2];

    private int bodyIndex;
    private int scaleIndex;
    private int heightIndex;
    private int locIndex;


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
    }

    // Update is called once per frame
    void Update()
    {

        if(changeScaleAction.triggered)
        {

            scaleIndex = Mathf.Min(scaleIndex + 1, 2);

            gameObject.transform.localScale = scaleChanges[scaleIndex];

           
  
        }

        if (changeHeightAction.triggered) 
        {
            heightIndex = Mathf.Min(heightIndex + 1, 2);
    
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, heightValues[heightIndex], gameObject.transform.localPosition.z);
        }

        if (changeLocationAction.triggered) 
        {
            locIndex = Mathf.Min(locIndex + 1, 2);

            Vector3 locPos = locations[locIndex].position;
            gameObject.transform.position = new Vector3(locPos.x, heightValues[heightIndex], locPos.z);
        }

        if (changeBodyAction.triggered) 
        {
            bodyIndex = Mathf.Min(bodyIndex + 1, 2);

            if(bodyIndex == 1) 
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

            
        }

        
    }
}

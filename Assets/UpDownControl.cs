using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownControl : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject MasterScriptObject;
    [SerializeField] private int direction;

    [SerializeField] private GameObject[] elements = new GameObject[4];

    private AudioSource source;
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        source.Play();

        Debug.Log("Direction");
        for(int i = 0; i < elements.Length; i++) 
        {
            if (elements[i].GetComponent<UIControls>().highlighted) 
            {
                changeLevel(elements[i].name);
            }
        }
    }

    private void changeLevel(string elementName) 
    {
        if(elementName == "Scale") 
        {
            MasterScriptObject.GetComponent<MasterController>().changeScale(direction);
        }
        if (elementName == "Height")
        {
            MasterScriptObject.GetComponent<MasterController>().changeHeight(direction);
        }
        if (elementName == "Location")
        {
            MasterScriptObject.GetComponent<MasterController>().changeLocation(direction);
        }
        if (elementName == "Body")
        {
            MasterScriptObject.GetComponent<MasterController>().changeBody(direction);
        }
    }
}

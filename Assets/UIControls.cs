using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControls : MonoBehaviour
{

    [SerializeField] private GameObject[] buttons = new GameObject[3];


    [System.NonSerialized]public bool highlighted;

    private AudioSource source;
 
    // Start is called before the first frame update
    void Start()
    {
       highlighted = false;
       source = gameObject.GetComponent<AudioSource>();
       
    }

    // Update is called once per frame
    void Update()
    {

       
        
    }
    private void OnTriggerEnter(Collider other)
    {
        source.Play();

        Debug.Log("Collided with " + name);
        highlighted = !highlighted;
        transform.GetChild(0).gameObject.SetActive(highlighted  );
        for (int i = 0; i < buttons.Length; i++) 
        {
            buttons[i].GetComponent<UIControls>().highlighted = false;
            buttons[i].transform.GetChild(0).gameObject.SetActive(false);

            
        }
    }
}

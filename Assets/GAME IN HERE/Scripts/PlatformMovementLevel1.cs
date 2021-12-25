using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementLevel1 : MonoBehaviour
{
    float ampH = 0.0f;
    float ampV = 0.0f;
    int state = 0;
    
    void Start()
    {
        // Horizontal movement amplitude
        ampH = Random.Range(-60.0f, 60.0f);
       
        // Vertical movement amplitude
        ampV = Random.Range(-10.0f, 10.0f);

        // Pick a random color for the platform
        GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f, 0.8f,0.8f);

        // Decide a random movement axis of platform (x or y)
        state = Random.Range(0, 2);

        // Use to pass level quickly (Ex:Debugging)
        //ampH = Random.Range(0.0f,0.0f);
        //ampV = Random.Range(0.0f,0.0f);
    }

	void Update () 
	{
        if(state == 0)
        {
            //Vertical moving platforms
            transform.position = new Vector3 (transform.position.x,ampV*Mathf.Sin(Time.fixedTime*0.5f), transform.position.z);
        }

        if(state == 1){
            // Horizontal moving platforms
            transform.position = new Vector3 (ampH*Mathf.Sin(Time.fixedTime*0.5f), -1 , transform.position.z);
        }
    }
}

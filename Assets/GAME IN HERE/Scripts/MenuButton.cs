using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    int counter = 0;
    float amp = 0;
    float timeSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        amp = Random.Range(Screen.width*0.15f, Screen.width*0.32f);
        timeSpeed = Random.Range(0.5f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {

        //transform.position = new Vector3 (Screen.width*0.5f + amp*Mathf.Sin(Time.fixedTime*timeSpeed), transform.position.y, transform.position.z);

        counter++;
        if(counter == 70)
        {
            transform.position = new Vector3 (Screen.width*0.5f + Random.Range(-Screen.width*0.1f, Screen.width*0.1f), transform.position.y, 0);
            counter = 0;
        }
    }
}

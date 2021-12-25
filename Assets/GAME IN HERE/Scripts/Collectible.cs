using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
	// Horizontal position amplitude
    float ampH = 0.0f;
	
	// Collectibles movement
	float ampA;

	Vector3 firstPosition;

    void Start()
    {
        ampH = Random.Range(-40.0f, 40.0f);
		ampA = 3;
		transform.position = new Vector3 (ampH, transform.position.y, transform.position.z);
		firstPosition = transform.position;
    }

	void Update () 
	{
		// Rotate the game object that this script is attached to by 15 in the X axis,
		// 30 in the Y axis and 45 in the Z axis, multiplied by deltaTime in order to make it per second
		// rather than per frame.
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
		transform.position = new Vector3 (firstPosition.y + 2.0f*ampH*Mathf.Sin(Time.fixedTime), firstPosition.y + ampA*Mathf.Sin(Time.fixedTime), transform.position.z);
    }
}

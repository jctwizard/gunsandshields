using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{

	public float velocity = 1.0f;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Couple of lines for the sun to go over head with a cos wave
		//float yPos = (transform.position.x / 11);
		//transform.position = new Vector3(transform.position.x + velocity*Time.deltaTime, Mathf.Cos(yPos)*100, 0.0f);

		transform.position = new Vector3 (transform.position.x + velocity * Time.fixedDeltaTime, (-((transform.position.x * transform.position.x) / 50.0f * 2.0f) + 5.0f), -2.0f);

		if (transform.position.x > 30.0f)
		{
			transform.position = new Vector3 (-30.0f, (-((transform.position.x * transform.position.x) / 15.0f * 2.0f) + 6.0f), 0.0f);
		}

	}
}

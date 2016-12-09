using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "shield")
		{
			GameObject.Destroy(gameObject);
		}
		else if (collider.gameObject.tag == "edge")
		{
			GameObject.Destroy(gameObject);
		}
	}
}

using UnityEngine;
using System.Collections;

public enum projectileType {standard, reflecting};

public class ProjectileScript : MonoBehaviour {

	public float reflectVelocity;

	public projectileType projType;

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
			if (projType == projectileType.reflecting)
			{
				if (gameObject.GetComponent<Rigidbody2D> ().velocity.x > 0.0f)
				{
					gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector3 (-reflectVelocity, gameObject.GetComponent<Rigidbody2D> ().velocity.y, 0.0f);
				}
				else if (gameObject.GetComponent<Rigidbody2D> ().velocity.x < 0.0f)
				{
					gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector3 (reflectVelocity, gameObject.GetComponent<Rigidbody2D> ().velocity.y, 0.0f);
				}
				else if (gameObject.GetComponent<Rigidbody2D> ().velocity.x == 0)
				{
					GameObject.Destroy (gameObject);
				}
			}

			if (projType == projectileType.standard)
			{
				GameObject.Destroy (gameObject);
			}
		}
		else if (collider.gameObject.tag == "edge")
		{
			GameObject.Destroy(gameObject);
		}
	}
}

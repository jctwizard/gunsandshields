using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float maxSpeed = 10.0f;
	public float acceleration = 1.0f;
	public bool grounded = false;
	public int lives = 3;
	public Vector3 initialPosition;
	public float jumpForce = 50.0f;

	public KeyCode jumpButton;
	public KeyCode stanceButton;
	public KeyCode shootButton;
	public KeyCode leftButton;
	public KeyCode rightButton;

	public GameObject gun;
	public GameObject shield;
	public float gunOffsetHorizontal = 0.5f;
	public float gunOffsetVertical = 0.5f;

	public GameObject projectile;
	public float projectileSpeed;

	public bool facingRight = true;
	public bool gunHigh = true;

	public GameObject opponent;

	public float shootCooldownDuration = 1.0f;
	public float shootCooldownTime = 1.0f;

	public float damage = 0.0f;
	public float knockBackForce = 10.0f;

	public Vector2 opponentBounceForce;

	public GameObject livesText;

	// Use this for initialization
	void Start () 
	{
		initialPosition = transform.position;
		gun.transform.localPosition = new Vector3(gunOffsetHorizontal, gunOffsetVertical, 0.0f);
		shield.transform.localPosition = new Vector3(gunOffsetHorizontal, -gunOffsetVertical, 0.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey(rightButton) && GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
		{
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * acceleration * Time.deltaTime);
		}

		if (Input.GetKey(leftButton) && GetComponent<Rigidbody2D>().velocity.x > -maxSpeed)
		{
			GetComponent<Rigidbody2D>().AddForce(-Vector2.right * acceleration * Time.deltaTime);
		}

		if (Input.GetKeyDown(jumpButton) && grounded)
		{
			GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
			grounded = false;
		}

		if (Input.GetKeyDown(shootButton) && grounded && shootCooldownTime >= shootCooldownDuration)
		{
			GameObject newProjectile = GameObject.Instantiate(projectile, gun.transform.position, Quaternion.identity) as GameObject;

			if (facingRight)
			{
				newProjectile.GetComponent<Rigidbody2D>().velocity = new Vector3(projectileSpeed, 0.0f, 0.0f);
			} 
			else
			{
				newProjectile.GetComponent<Rigidbody2D>().velocity = new Vector3(-projectileSpeed, 0.0f, 0.0f);
			}

			shootCooldownTime = 0.0f;
		}

		if (Input.GetKeyDown(stanceButton))
		{
			if (gunHigh)
			{
				gunHigh = false;
				gun.transform.localPosition = new Vector3(gun.transform.localPosition.x, -gunOffsetVertical, 0.0f);
				shield.transform.localPosition = new Vector3(shield.transform.localPosition.x, gunOffsetVertical, 0.0f);
			}
			else
			{
				gunHigh = true;
				gun.transform.localPosition = new Vector3(gun.transform.localPosition.x, gunOffsetVertical, 0.0f);
				shield.transform.localPosition = new Vector3(shield.transform.localPosition.x, -gunOffsetVertical, 0.0f);
			}
		}

		if (opponent.transform.position.x > transform.position.x && facingRight == false)
		{
			facingRight = true;
			gun.transform.localPosition = new Vector3(gunOffsetHorizontal, gun.transform.localPosition.y, 0.0f);
			shield.transform.localPosition = new Vector3(gunOffsetHorizontal, shield.transform.localPosition.y, 0.0f);
		}

		if (opponent.transform.position.x < transform.position.x && facingRight == true)
		{
			facingRight = false;
			gun.transform.localPosition = new Vector3(-gunOffsetHorizontal, gun.transform.localPosition.y, 0.0f);
			shield.transform.localPosition = new Vector3(-gunOffsetHorizontal, shield.transform.localPosition.y, 0.0f);
		}

		if (shootCooldownTime < shootCooldownDuration)
		{
			shootCooldownTime += Time.deltaTime;
		}
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if (collider.gameObject.tag == "platform")
		{
			grounded = true;
		}
		else if (collider.gameObject.tag == "projectile")
		{
			damage += 10;

			if (collider.gameObject.transform.position.x > transform.position.x)
			{
				GetComponent<Rigidbody2D>().AddForce(-Vector2.right * knockBackForce * damage);
			}
			else
			{
				GetComponent<Rigidbody2D>().AddForce(Vector2.right * knockBackForce * damage);
			}

			GameObject.Destroy(collider.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "death")
		{
			lives -= 1;

			livesText.GetComponent<TextMesh>().text = lives.ToString();

			transform.position = initialPosition;
			GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
			GetComponent<Rigidbody2D>().angularVelocity = 0.0f;

			damage = 0.0f;

			// The opposite player won - reset the game.
			if (lives == 0)
			{
				lives = 3;
				collider.gameObject.GetComponent<PlayerController>().lives = 3;
				collider.gameObject.transform.position = collider.gameObject.GetComponent<PlayerController>().initialPosition;
				collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
				collider.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
			}
		}
		// Made this a trigger collider and moved the collider box to be slightly bigger than the player to avoid interacting with the physics engine on player collisions
		else if (collider.gameObject.tag == "player")
		{
			// We only want the fastest moving player to bounce away
			if (Mathf.Abs(GetComponent<Rigidbody2D> ().velocity.x) >= Mathf.Abs(collider.gameObject.GetComponent<Rigidbody2D> ().velocity.x))
			{
				GetComponent<Rigidbody2D> ().velocity = new Vector3 (-GetComponent<Rigidbody2D> ().velocity.x * opponentBounceForce.x, GetComponent<Rigidbody2D> ().velocity.y + opponentBounceForce.y, 0.0f);
			}

			// Should we be able to jump off the other player?
			// Either should be able to land on the other player then jump off by setting grounded to true,
			// or 'bounce' off the top of the other player somehow by having opponentBounceForce.y > 0
			//grounded = true;
		}
	}
}

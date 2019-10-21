using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

	public float runningSpeed = 10f;
	public float walkingSpeed = 2f;
	float speed = 0f;
	public float jumpSpeed = 4.5f;
	Vector3 direction = Vector3.zero;
	float verticalVelocity = 0;
	CharacterController cc;
	Animator anim;

	bool crouching;
	bool walking;

	void Start()
	{
		cc = GetComponent<CharacterController> ();
		anim = GetComponent<Animator> ();
		crouching = false;
		walking = false;
	}

	void Update ()
	{
		//WASD forward/back & left/right movement is stored in "direction".
		direction = transform.rotation * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		if (direction.magnitude > 1f)
		{
			direction = direction.normalized;
		}

		//Check if we are walking(shift) and update animation walk parameter
		if(Input.GetKeyDown(KeyCode.LeftShift) && walking == false)
		{
			walking = true;
			anim.SetBool("Walking", true);
		}
		if(Input.GetKeyUp(KeyCode.LeftShift) && walking == true)
		{
			walking = false;
			anim.SetBool("Walking", false);
		}

		//Check if we are crouching(ctrl) and update animation crouch parameter
		if(Input.GetKeyDown(KeyCode.LeftControl) && crouching == false)
		{
			crouching = true;
			anim.SetBool("Crouching", true);
		}
		if(Input.GetKeyUp(KeyCode.LeftControl) && crouching == true)
		{
			crouching = false;
			anim.SetBool("Crouching", false);
		}

		//Update current speed accordingly
		if(crouching || walking)
		{
			speed = walkingSpeed;
		}
		else
		{
			speed = runningSpeed;
		}

		//Update animation speed parameter
		anim.SetFloat ("Speed", direction.magnitude);

		//Jumping
		if (cc.isGrounded && Input.GetButtonDown ("Jump"))
		{
			verticalVelocity = jumpSpeed;
		}
	}

	void FixedUpdate()
	{
		Vector3 dist = direction * speed * Time.deltaTime;

		if (cc.isGrounded && verticalVelocity < 0)
		{
			//Make sure jump animation is not playing
			anim.SetBool("Jumping", false);

			//Set a negative velocity
			verticalVelocity = Physics.gravity.y * Time.deltaTime;
		}
		else
		{
			//Not touching ground/vertical velocity > 0 - starting to jump
			if(Mathf.Abs(verticalVelocity) > jumpSpeed * 0.75f) //Make sure you actually fall, not just take a step down
			{
				anim.SetBool("Jumping", true);
			}
			//Apply gravity
			verticalVelocity += Physics.gravity.y * Time.deltaTime;
		}

		//Add vertical velocity to actual movement for this frame
		dist.y = verticalVelocity * Time.deltaTime;

		//Apply the movement to our character
		cc.Move (dist);
	}
}

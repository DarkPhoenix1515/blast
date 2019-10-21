using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]

public class FPController : MonoBehaviour {
	
	public float runSpeed = 10.0f;
	public float jumpSpeed = 6.0f;
	public float mouseSensivity = 1.0f;
	public float verticalRotationLimit = 90.0f;
	float verticalRotation = 0;
	
	float verticalVelocity = 0.0f;
	CharacterController characterController;

	void Start () 
	{
		characterController = GetComponent<CharacterController>();
		Screen.lockCursor = true;
	}
	
	void Update () 
	{
		//Rotation
		float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensivity;
		transform.Rotate(0, horizontalRotation, 0);
		
		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensivity;
		
		//Clamp rotation
		verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
		
		//Actual rotation
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
		
		
		//Movement
		float forwardSpeed = Input.GetAxis("Vertical") * runSpeed;
		float sideSpeed = Input.GetAxis("Horizontal") * runSpeed;
		
		if(!characterController.isGrounded)
		{
			//da-i full speed in aer, dar taie viteza cand aterizeaza
			forwardSpeed = forwardSpeed * 2/3;
			sideSpeed = sideSpeed * 2/3;
		}
		
		//Jump
		verticalVelocity += Physics.gravity.y * Time.deltaTime;
		if(characterController.isGrounded && Input.GetButtonDown("Jump"))
		{
			verticalVelocity = jumpSpeed;
		}
		
		Vector3 directionVector = new Vector3(forwardSpeed, verticalVelocity, sideSpeed);
		
		directionVector = transform.rotation * directionVector * Time.deltaTime;
		characterController.Move(directionVector);
	}
}

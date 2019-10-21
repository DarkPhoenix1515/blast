using UnityEngine;

public class FirstPersonController : MonoBehaviour {
	public enum characterStates {Idle, Walking, Running, Air};
	public enum throwForce {high, medium, low};
	public characterStates state { get { return state; } set {} }

	public bool useController = false;
	public float runSpeed = 7.0f;
	public float walkSpeed = 3.5f;
	public float sensitivity = 1f;
	public float rotVerticalRange = 90.0f;
	public float jumpSpeed = 5.0f;

	GameObject playerCamera;
	CharacterController characterController;
	WeaponController weapons;
	bool alive = true;
	float verticalVelocity = 0;
	float previousAltitude;
	float rotVertical = 0f;
	float rotVerticalValue = 0f;
	float rotHorizontal = 0f;
	float forwardSpeed = 0f;
	float sideSpeed = 0f;
	
	void Awake () {
		playerCamera = GameObject.Find("PlayerCamera");
		characterController = GetComponent<CharacterController>();
		weapons = GetComponent<WeaponController>();
		previousAltitude = transform.position.y;
	}

	void updateMovement() {
		// Apply player movement
		Vector3 speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);
		speed =  transform.rotation * speed;
		characterController.Move(speed * Time.deltaTime);

		if (characterController.isGrounded) {
			verticalVelocity = Physics.gravity.y;
		} else {
			verticalVelocity += Physics.gravity.y * Time.deltaTime;
			state = characterStates.Air;
		}
	}

	void updateInputKeyboardMouse () {
		// TODO: Add buttons input

		// Get keyboard input
		forwardSpeed = Input.GetAxis("Vertical") * runSpeed;
		sideSpeed = Input.GetAxis("Horizontal") * runSpeed;

		if (characterController.isGrounded && Input.GetButtonDown("Jump")) {
			verticalVelocity = jumpSpeed;
		}

		// TODO: switch to ButtonUp in order to be able to "cook" and implement a medium throw
		// on button down, "cook" the grenade; on button up throw
		if (Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1)) { // mid throw
			weapons.attack(throwForce.medium.ToString());
		} else {
			if (Input.GetMouseButtonDown (0)) {
				weapons.attack (throwForce.high.ToString ());
			}

			if (Input.GetMouseButtonDown(1)) {
				weapons.attack(throwForce.low.ToString());
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			weapons.switchToWeapon(0);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			weapons.switchToWeapon(1);
		}

		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			weapons.switchToWeapon(2);
		}

		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			weapons.switchToWeapon(3);
		}

		if (Input.GetKeyDown(KeyCode.Alpha5)) {
			weapons.switchToWeapon(4);
		}
	}

	/// <summary>
	/// Get mouse input and apply player rotation
	/// </summary>
	void updateMouseMovement () {
		rotHorizontal = Input.GetAxis("Mouse X") * sensitivity;
		transform.Rotate(0, rotHorizontal, 0);

		rotVerticalValue = -Input.GetAxis("Mouse Y") * sensitivity;
		rotVertical += rotVerticalValue;
		rotVertical = Mathf.Clamp(rotVertical, -rotVerticalRange, rotVerticalRange);
		playerCamera.transform.localRotation = Quaternion.Euler(rotVertical, 0, 0);
	}
	
	void FixedUpdate () {
		if (alive)
			updateMovement();
	}

	void Update () {
		updateMouseMovement(); // mouse movement must me updated ASAP
		updateInputKeyboardMouse();
	}

	public void die () {
		this.alive = false;
	}
}

using UnityEngine;

public class FirstPersonControllerAI : MonoBehaviour {
	public enum characterStates {Idle, Walking, Running, Air};
//	public enum throwForce {high, medium, low};
	public characterStates state { get { return state; } set {} }

	public float runSpeed = 7.0f;
	public float walkSpeed = 3.5f;
	public float sensitivity = 3.5f;
	public float rotVerticalRange = 90.0f;
	public float jumpSpeed = 5.0f;

	GameObject playerCamera;
	CharacterController characterController;
	WeaponControllerAI weapons;
	struct CharacterProperties {
		public float verticalVelocity;
		public float previousAltitude;
		public float rotVertical;
		public float rotVerticalValue;
		public float rotHorizontal;
		public float forwardSpeed;
		public float sideSpeed;
	};
	CharacterProperties cProperties;

	public struct InputData {
		public float rotHorizontal;
		public float rotVerticalValue;
		public float moveForward;
		public float moveSide;
		public bool fire1;
		public bool fire2;
		public bool jump;
	};
	InputData input;

	void Awake () {
		initializeProperties();
	}

	void updateMovement() {
		// Apply character rotation
		transform.Rotate(0, input.rotHorizontal, 0);
		cProperties.rotVertical += input.rotVerticalValue;
		cProperties.rotVertical = Mathf.Clamp(cProperties.rotVertical, -rotVerticalRange, rotVerticalRange);
		playerCamera.transform.localRotation = Quaternion.Euler(cProperties.rotVertical, 0, 0);

		// Apply character movement
		Vector3 speed = new Vector3(cProperties.sideSpeed, cProperties.verticalVelocity, cProperties.forwardSpeed);
		cProperties.verticalVelocity = Physics.gravity.y;
		speed = transform.rotation * speed;
		characterController.Move(speed * Time.deltaTime);
	}

	void applyInput () {
		cProperties.rotHorizontal = input.rotHorizontal * sensitivity;
		cProperties.rotVerticalValue = input.rotVerticalValue * sensitivity;
		cProperties.forwardSpeed = input.moveForward * runSpeed;
		cProperties.sideSpeed = input.moveSide * runSpeed;

		if (characterController.isGrounded && input.jump) {
			cProperties.verticalVelocity = jumpSpeed;
		}

		if (input.fire1) {
			weapons.attack ();
		} else {
			if (input.fire2) {
				weapons.attack();
			}
		}
	}

	public void updateInputAI(InputData data) {
		this.input = data;
		applyInput();
		updateMovement();
	}

	void Update () {

	}

	public void reset () {
		cProperties.verticalVelocity = 0;
		cProperties.previousAltitude = transform.position.y;
		cProperties.rotVertical = 0f;
		cProperties.rotVerticalValue = 0f;
		cProperties.rotHorizontal = 0f;
		cProperties.forwardSpeed = 0f;
		cProperties.sideSpeed = 0f;

		input.rotVerticalValue = 0f;
		input.rotHorizontal = 0f;
		input.moveForward = 0f;
		input.moveSide = 0f;
		input.jump = false;
		input.fire1 = false;
		input.fire2 = false;
	}

	void initializeProperties () {
		playerCamera = transform.Find("Arms").gameObject;
		characterController = GetComponent<CharacterController>();
		weapons = GetComponent<WeaponControllerAI>();

		cProperties.verticalVelocity = 0;
		cProperties.previousAltitude = transform.position.y;
		cProperties.rotVertical = 0f;
		cProperties.rotVerticalValue = 0f;
		cProperties.rotHorizontal = 0f;
		cProperties.forwardSpeed = 0f;
		cProperties.sideSpeed = 0f;

		input.rotVerticalValue = 0f;
		input.rotHorizontal = 0f;
		input.moveForward = 0f;
		input.moveSide = 0f;
		input.jump = false;
		input.fire1 = false;
		input.fire2 = false;
	}
}

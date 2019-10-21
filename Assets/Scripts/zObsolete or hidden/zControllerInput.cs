
public class zControllerInput {
//	void updateInputController() {
//		/*
//		if (Input.GetAxis("DPADHorizontal") > 0) {
//			Debug.Log ("DPADHorizontal +");
//		} else 
//			if (Input.GetAxis("DPADHorizontal") < 0)
//				Debug.Log ("DPADHorizontal -");
//
//		if (Input.GetAxis("DPADVertical") > 0) {
//			Debug.Log ("DPADVertical +");
//		} else 
//			if (Input.GetAxis("DPADVertical") < 0)
//				Debug.Log ("DPADVertical -");
//
//		if (Input.GetAxis("TriggerLeft") > 0) {
//			Debug.Log ("TriggerLeft");
//		}
//		if (Input.GetAxis("TriggerRight") > 0)
//			Debug.Log ("TriggerRight");
//		if (Input.GetButtonDown("ButtonA"))
//			Debug.Log ("ButtonA");
//		if (Input.GetButtonDown("ButtonB"))
//			Debug.Log ("ButtonB");
//		if (Input.GetButtonDown("ButtonX"))
//			Debug.Log ("ButtonX");
//		if (Input.GetButtonDown("ButtonY"))
//			Debug.Log ("ButtonY");
//		if (Input.GetButtonDown("BumperLeft"))
//			Debug.Log ("BumperLeft");
//		if (Input.GetButtonDown("BumperRight"))
//			Debug.Log ("BumperRight");
//		if (Input.GetButtonDown("View"))
//			Debug.Log ("View");
//		if (Input.GetButtonDown("Start"))
//			Debug.Log ("Start");
//		if (Input.GetButtonDown("ButtonLeftStick"))
//			Debug.Log ("ButtonLeftStick");
//		if (Input.GetButtonDown("ButtonRightStick"))
//			Debug.Log ("ButtonRightStick");*/
//
//		cProperties.rotHorizontal = Input.GetAxis("RightJoystickHorizontal") * sensitivity;
//		cProperties.rotVerticalValue = Input.GetAxis("RightJoystickVertical") * sensitivity;
//
//		float rawVertical = Input.GetAxis ("Vertical");
//		float rawHorizontal = Input.GetAxis ("Horizontal");
//		cProperties.forwardSpeed = Mathf.Abs(rawVertical) < 0.2f ? 0 : rawVertical * runSpeed;
//		cProperties.sideSpeed = Mathf.Abs(rawHorizontal) < 0.2f ? 0 : rawHorizontal * runSpeed;
//
//		if (Mathf.Abs(rawVertical) <= 0.1 && Mathf.Abs(rawHorizontal) <= 0.1) {
//			state = characterStates.Idle;
//		} else if (Mathf.Abs(rawVertical) <= 0.5 && Mathf.Abs(rawHorizontal) <= 0.5) {
//			state = characterStates.Walking;
//		} else {
//			state = characterStates.Running;
//		}
//
//		if (characterController.isGrounded && Input.GetAxis("Jump") > 0) {
//			cProperties.verticalVelocity = jumpSpeed;
//		}
//	}
}

using UnityEngine;

public class ControlInterface : MonoBehaviour {
	FirstPersonControllerAI fpc;
	FirstPersonControllerAI.InputData input;

	void Awake () {
		this.fpc = GetComponent<FirstPersonControllerAI>();
	}

	void Update () {
		
	}

	// prob will have to switch function params to variables and create a InputData struct to pass on
	public void updateInput (FirstPersonControllerAI.InputData data) {
		this.input = data;
		// process data?
		fpc.updateInputAI(this.input);
	}
}

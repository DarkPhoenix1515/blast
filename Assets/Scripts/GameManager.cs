using UnityEngine;

public class GameManager : MonoBehaviour {

	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update () {
		updateInput ();
	}

	void updateInput () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

	void Awake () {
		gameObject.GetComponent<HasHealth>().death = this.die;
	}
	
	void die() {
		// add reward
		Destroy(gameObject);
	}
}

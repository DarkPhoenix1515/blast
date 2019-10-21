using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour {
	public float delay = 1f;

	void Start () {
		Invoke ("cleanUp", delay);
	}

	void cleanUp () {
		Destroy (gameObject);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
	public float lifeTime = 1f;
	float radius = 1f;

	void Start () {
		Invoke("cleanUp", lifeTime);
	}

	public void initialize (float radius) {
		this.radius = radius;
	}
	
	void cleanUp () {
		Destroy(gameObject);
	}

}

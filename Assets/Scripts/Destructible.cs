using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {
	public GameObject Debris;

	public float maxHealth = 500f;
	float currentHealth;
	bool alive = true;

	void Awake () {
		currentHealth = maxHealth;	
	}

	public void takeDamage (float amount) {
		if (alive) {
			currentHealth -= amount;

			if (currentHealth <= 0) {
				die();
			}
		}
	}

	public void die () {
		GameObject debris = Instantiate(Debris, transform.position, transform.rotation);
		debris.transform.SetParent(gameObject.transform.parent);
		alive = false;
		Destroy(gameObject);
	}
}

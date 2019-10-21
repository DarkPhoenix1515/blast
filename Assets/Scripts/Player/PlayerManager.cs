using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	bool alive = true;
	FirstPersonController fpController;
	HasHealth health;

	void Awake () {
		fpController = gameObject.GetComponent<FirstPersonController>();
		health = gameObject.GetComponent<HasHealth>();
		health.death = this.die;
	}

//	public void takeDamage (float amount) {
//		if (alive) {
//			currentHealth -= amount;
//			Debug.Log (gameObject.name + " took " + amount + " damage");
//
//			if (currentHealth <= 0) {
//				die();
//			}
//		}
//	}

	public void die () {
		alive = false;
		fpController.die();
		Debug.Log (gameObject.name + " died");
		// hide player graphics and replace with ragdoll
	}
}

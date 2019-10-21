using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeDetector : MonoBehaviour {
	public delegate void addGrenade(GameObject grenade);
	public delegate void removeGrenade(GameObject grenade);
	public addGrenade addObservedGrenade;
	public addGrenade removeObservedGrenade;

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.tag == "grenade") {
			addObservedGrenade(collider.gameObject);
		}
	}

	void OnTriggerExit (Collider collider) {
		if (collider.gameObject.tag == "grenade") {
			removeObservedGrenade(collider.gameObject);
		}
	}
}

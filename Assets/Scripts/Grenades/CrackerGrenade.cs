using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackerGrenade : Grenade {

	public override void initialize (WeaponData data, GameObject source) {
		damage = data.damage;
		explosionRadius = data.explosionRadius;
		explosionForce = data.explosionForce;
		this.source = source;
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.tag != "grenadeDetector") {
			explode();
		}
	}
}

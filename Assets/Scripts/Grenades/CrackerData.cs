using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackerData : WeaponData {

	public CrackerData () {
		delay = 0f;
		damage = 100f;
		explosionForce = 700f;
		explosionRadius = 3f;
	}

	public override void toggleUp () {
		// should the cracker be able to have a one-bounce mode?
	}

	public override void toggleDown () {
		
	}
}


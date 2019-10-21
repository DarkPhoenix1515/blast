using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerData : WeaponData {
	const float maxTime = 5f;
	const float minTime = 1f;

	public TimerData () {
		delay = 3f;
		damage = 50f;
		explosionForce = 700f;
		explosionRadius = 3f;
	}

	public override void toggleUp () {
		delay = delay + 1 > maxTime ? maxTime : delay + 1;
	}

	public override void toggleDown () {
		delay = delay - 1 < minTime ? minTime : delay - 1;
	}
}

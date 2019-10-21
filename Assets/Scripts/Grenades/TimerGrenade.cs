using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerGrenade : Grenade {
	protected float countdown;
	protected float delay = 1f;

	void Start () {

	}

	public override void initialize (WeaponData data, GameObject source) {
		damage = data.damage;
		explosionRadius = data.explosionRadius;
		explosionForce = data.explosionForce;
		delay = data.delay;
		countdown = delay;
		this.source = source;
	}

	void Update () {
		countdown -= Time.deltaTime;

		if (countdown <= 0) {
			explode ();
		}
	}
}

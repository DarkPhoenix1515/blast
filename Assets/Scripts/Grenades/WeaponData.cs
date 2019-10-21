using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponData {
	public float damage { get; set;}
	public float explosionRadius { get; set;}
	public float explosionForce { get; set;}
	public float delay { get; set;}

	public abstract void toggleUp();
	public abstract void toggleDown();
}

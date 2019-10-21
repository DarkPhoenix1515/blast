using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Grenade : MonoBehaviour {
	public GameObject explosionPrefab;
	public float explosionRadius = 3f;
	public float explosionForce = 700f;
	public float damage = 50f;
	protected bool triggered = false;
	protected GameObject source;

	void Awake () {

	}

	void Start () {
		
	}

	public void trigger () {
		this.triggered = true;
	}

	public abstract void initialize (WeaponData data, GameObject source);

	protected void explode () {
		GameObject exp = Instantiate (explosionPrefab, transform.position, Quaternion.identity);
		Explosion explosion = exp.GetComponent<Explosion>();
		explosion.transform.SetParent(transform.parent);
		explosion.initialize(explosionRadius);
		int layerMask = LayerMask.GetMask("Player");
		Collider[] playerColliders = Physics.OverlapSphere (transform.position, explosionRadius, layerMask);

		foreach (Collider col in playerColliders) {
//			PlayerManagerAI pma = col.GetComponent<PlayerManagerAI>();
//
//			if (pma != null) {
//				pma.takeDamage(damage);
//			} else {
//				PlayerManager pm = col.GetComponent<PlayerManager>();
//
//				if (pm != null) {
//					pm.takeDamage(damage);
//				}
//			}
			HasHealth hh = col.GetComponent<HasHealth>();

			if (hh != null) {
				hh.takeDamage((int)damage, source);
			}


		}
		layerMask = LayerMask.GetMask("Destructible");
		Collider[] destructibleColliders = Physics.OverlapSphere (transform.position, explosionRadius, layerMask);

		foreach (Collider col in destructibleColliders) {
			Rigidbody rb = col.GetComponent<Rigidbody>();

			if (rb != null) {
				rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
			}
			Destructible des = col.GetComponent<Destructible>();

			if (des != null) {
				des.takeDamage(damage);
			}

			/*
			 * Health h = col.GetComponent<Health>();
			 * 
			 * if (h != null) {
			 * 	h.takeDamage (damage);
			 * }
			*/
		}
		cleanUp ();
	}

	void cleanUp () {
		Destroy (gameObject);
	}
}

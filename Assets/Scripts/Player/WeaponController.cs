using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
	GameObject playerCamera;
	public GameObject firePoint;
	public GameObject timerText;
	[Serializable]
	public struct Weapon {
		public string name;
		public GameObject prefab;
		public WeaponData data;
		public int count;
	}

	public Weapon[] weapons;
	public GameObject selectedWeapon;
	public GameObject[] weaponGraphics;

	public float throwForceHigh = 20f;
	public float throwForceMedium = 14f;
	public float throwForceLow = 8f;

	bool grenadeEquipped = false;
	int currentWeaponSlot = 0;
	int previousWeaponSlot = 0;
	Dictionary<string, float> attackForce = new Dictionary<string, float>();
	GameObject grenadeParent;
	float weaponCooldown = 0.5f;
	float currentCooldown;

	void Awake () {
		initialize();
	}

	void Start () {
		switchToWeapon(1);
		// instantiate a knife prefab and toggle it on/off
	}

	public void attack (string forceMode) {
		if (currentCooldown <= 0) {
			currentCooldown = weaponCooldown;

			if (grenadeEquipped) {
				throwGrenade (attackForce[forceMode]);
			} else {
				meleeAttack ();
			}
		}
	}

	void throwGrenade (float throwForce) {
		GameObject grenade = Instantiate (selectedWeapon, firePoint.transform.position, playerCamera.transform.rotation);
		Rigidbody grenadeRigidbody = grenade.GetComponent<Rigidbody>();
		Vector3 playerVelocity = gameObject.GetComponent<CharacterController>().velocity;

		grenade.GetComponent<Grenade>().initialize(weapons[currentWeaponSlot].data, gameObject);
		grenadeRigidbody.AddForce(grenade.transform.forward * throwForce + playerVelocity, ForceMode.VelocityChange);
		grenade.transform.SetParent(grenadeParent.transform);
		weapons[currentWeaponSlot].count -= 1;

		if (weapons[currentWeaponSlot].count == 0) {
			previousSlot();
		}
	}

	void meleeAttack () {
		Debug.Log("melee attack");
	}
	
	public void switchToWeapon (int weaponSlot) {
		if (weaponSlot > 4 || weapons[weaponSlot].prefab == null || weaponSlot == currentWeaponSlot) {
			return;
		}

		if (weaponSlot == 0) {
			grenadeEquipped = false;
		} else {
			if (weapons[weaponSlot].count <= 0) {
				return;
			} else {
				grenadeEquipped = true;
			}
		}

		previousWeaponSlot = currentWeaponSlot;
		currentWeaponSlot = weaponSlot;
		selectedWeapon = weapons[weaponSlot].prefab;
		switchWeaponGraphics();
	}

	void switchWeaponGraphics () {
		if (weaponGraphics[currentWeaponSlot] == null) {
			Debug.Log("no weapon graphics");
			return;
		}
		weaponGraphics[previousWeaponSlot].SetActive(false);
		weaponGraphics[currentWeaponSlot].SetActive(true);
	}

	public void previousSlot () {
		if (previousWeaponSlot != 0 || weapons[previousWeaponSlot].count <= 0) {
				switchToWeapon (0);
		} else {
			switchToWeapon (previousWeaponSlot);
		}
	}

	void Update () {
		updateInput();

		if (currentCooldown >= 0)
			currentCooldown -= Time.deltaTime;
	}

	void updateInput () {
		if (Input.GetButtonDown("ToggleUp")) {
			weapons[currentWeaponSlot].data.toggleUp();
			if (weapons[currentWeaponSlot].name == "Timer") {
				this.timerText.GetComponent<TextMesh>().text = weapons[currentWeaponSlot].data.delay.ToString();
			}
		}

		if (Input.GetButtonDown("ToggleDown")) {
			weapons[currentWeaponSlot].data.toggleDown();
			if (weapons[currentWeaponSlot].name == "Timer") {
				this.timerText.GetComponent<TextMesh>().text = weapons[currentWeaponSlot].data.delay.ToString();
			}
		}
	}

	void initialize () {
		playerCamera = GameObject.Find("PlayerCamera");
		attackForce.Add("low", throwForceLow);
		attackForce.Add("medium", throwForceMedium);
		attackForce.Add("high", throwForceHigh);
		grenadeParent = GameObject.FindGameObjectWithTag("grenadeParent");
		currentCooldown = weaponCooldown;

		for (int i = 0; i < weapons.Length; i++) {
			switch (weapons[i].name) {
				case "Cracker":
					weapons[i].data = new CrackerData();
					break;
				case "Timer":
					weapons[i].data = new TimerData();
					break;
			}
		}
	}
}

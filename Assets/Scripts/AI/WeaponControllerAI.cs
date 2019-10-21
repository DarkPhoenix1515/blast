using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControllerAI : MonoBehaviour {
	GameObject arms;
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

	float weaponCooldown = 0.5f;
	float currentCooldown = 0f;
	public float throwForceHigh = 30f;

	bool grenadeEquipped = true;
	int currentWeaponSlot = 0;
	int previousWeaponSlot = 0;
	GameObject grenadeParent;

	void Awake () {
		initialize();
	}

	void Start () {
		switchToWeapon(1);
	}

	public void attack () {
		if (currentCooldown <= 0) {
			throwGrenade ();
			currentCooldown = weaponCooldown;
		}
	}

	void throwGrenade () {
		GameObject grenade = Instantiate (selectedWeapon, firePoint.transform.position, arms.transform.rotation);
		Rigidbody grenadeRigidbody = grenade.GetComponent<Rigidbody>();
		Vector3 playerVelocity = gameObject.GetComponent<CharacterController>().velocity;

		grenade.GetComponent<Grenade>().initialize(weapons[currentWeaponSlot].data, gameObject);
		grenadeRigidbody.AddForce(grenade.transform.forward * throwForceHigh + playerVelocity, ForceMode.VelocityChange);
		grenade.transform.SetParent(grenadeParent.transform);

		// no weapon swap or count limit for now
//		weapons[currentWeaponSlot].count -= 1;
//
//		if (weapons[currentWeaponSlot].count == 0) {
//			previousSlot();
//		}
	}

	public void switchToWeapon (int weaponSlot) {
//		if (weaponSlot > 4 || weapons[weaponSlot].prefab == null || weaponSlot == currentWeaponSlot) {
//			return;
//		}
//
//		if (weaponSlot == 0) {
//			grenadeEquipped = false;
//		} else {
//			if (weapons[weaponSlot].count <= 0) {
//				return;
//			} else {
//				grenadeEquipped = true;
//			}
//		}
//
//		previousWeaponSlot = currentWeaponSlot;
		currentWeaponSlot = weaponSlot;
		selectedWeapon = weapons[weaponSlot].prefab;
//		switchWeaponGraphics();
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
		if (currentCooldown >= 0) {
			currentCooldown -= Time.deltaTime;
		}
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
		arms = transform.Find("Arms").gameObject;
		grenadeParent = GameObject.FindGameObjectWithTag("grenadeParent");
		grenadeEquipped = true;
		weapons[1].data = new CrackerData();

//		for (int i = 0; i < weapons.Length; i++) {
//			switch (weapons[i].name) {
//			case "Cracker":
//				weapons[i].data = new CrackerData();
//				break;
//			case "Timer":
//				weapons[i].data = new TimerData();
//				break;
//			}
//		}
	}
}

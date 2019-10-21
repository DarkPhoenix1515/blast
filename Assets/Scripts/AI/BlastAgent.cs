using System;
using UnityEngine;
using System.Collections.Generic;

public class BlastAgent : Agent {
	public GameObject ground;
	BlastAcademy academy;
	FirstPersonControllerAI fpc;
	GameObject arms;
	CharacterController agentCharacterController;
	public GameObject[] grenades;
	List<GameObject> grenadesBuffer;
	RayPerception rayPer;
	Bounds arenaBoundaries;
	const float arenaHeight = 50f;
	HasHealth health;
	List<Transform> spawnPoints;
	ScoreManager scoreManager;
	//	Vector3 velocityLimits = new Vector3(8f, 5f, 8f);

	void Awake() {
		academy = FindObjectOfType<BlastAcademy>();
		brain = FindObjectOfType<Brain>();
		rayPer = GetComponent<RayPerception>();
		fpc = gameObject.GetComponent<FirstPersonControllerAI>();
		arms = transform.Find("Arms").gameObject;
		ground = transform.parent.Find("Ground").gameObject;
		agentCharacterController = gameObject.GetComponent<CharacterController>();
		GrenadeDetector gd = gameObject.GetComponentInChildren<GrenadeDetector>();
		gd.addObservedGrenade = this.addObservedGrenade;
		gd.removeObservedGrenade = this.removeObservedGrenade;
		grenades = new GameObject[4];
		grenadesBuffer = new List<GameObject>();
		arenaBoundaries = ground.GetComponent<BoxCollider>().bounds;
		health = gameObject.GetComponent<HasHealth>();
		health.death = this.IFailed;
		spawnPoints = new List<Transform>();
		Transform spawnPointsParent = transform.parent.Find("SpawnPoints");

		foreach (Transform sp in spawnPointsParent) {
			spawnPoints.Add(sp);
		}
		transform.position = GetRandomSpawnPoint();
		gameObject.GetComponentInChildren<Renderer>().material.color = UnityEngine.Random.ColorHSV();

		if (academy.enableScoreboard) {
			scoreManager = GameObject.Find("GameManager").GetComponent<ScoreManager>();
		}
	}

	public override void CollectObservations() {
		// Agent state info
		// Normalize values
		float bodyAngle = gameObject.transform.eulerAngles.y/360f;
		float cameraAngle = (arms.transform.rotation.eulerAngles.x > 100f ? 450f - arms.transform.rotation.eulerAngles.x : 90f - arms.transform.rotation.eulerAngles.x)/180f;
		float xPosition = (transform.position.x - arenaBoundaries.min.x)/(arenaBoundaries.size.x);
		float yPosition = (transform.position.x)/(arenaHeight);
		float zPosition = (transform.position.z - arenaBoundaries.min.z)/(arenaBoundaries.size.z);
		Vector3 agentVelocity = agentCharacterController.velocity.normalized;
		// Add values as observations
		AddVectorObs(bodyAngle);
		AddVectorObs(cameraAngle);
		AddVectorObs(xPosition);
		AddVectorObs(yPosition);
		AddVectorObs(zPosition);
		AddVectorObs(agentVelocity);

		// Raycast info
		float rayDistance = 60;
		float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 110f, 70f };
		string[] detectableObjects = { "Agent", "wall", "ground", "ceiling", "grenade" };
		// fire a set of rays at feet level to detect obstacles
		AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0.2f, 0.2f));
		// perceive where Agent looks
		float armsEndOffset = Mathf.Sin(arms.transform.rotation.eulerAngles.x * Mathf.Deg2Rad) * rayDistance;
		armsEndOffset = (float)Math.Round(-armsEndOffset, 2); // invert sign as arms rotate clockwise
		AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1.7f, armsEndOffset));

		// Detect grenades
		for (int i = 0; i < grenades.Length; i++) {
			if (grenades[i] != null) {
				// Normalize grenades values
				float xGPosition = (grenades[i].transform.position.x - arenaBoundaries.min.x)/arenaBoundaries.size.x;
				float yGPosition = grenades[i].transform.position.y/arenaHeight;
				float zGPosition = (grenades[i].transform.position.z - arenaBoundaries.min.z)/arenaBoundaries.size.z;
				Vector3 gVelocity = grenades[i].GetComponent<Rigidbody>().velocity.normalized;
				AddVectorObs(xGPosition);
				AddVectorObs(yGPosition);
				AddVectorObs(zGPosition);
				AddVectorObs(gVelocity.x);
				AddVectorObs(gVelocity.y);
				AddVectorObs(gVelocity.z);
			} else {
				AddVectorObs(Vector3.zero);
				AddVectorObs(Vector3.zero);
			}
		}
	}

	/// <summary>
	/// Specifies the agent behavior at every step based on the provided
	/// action.
	/// </summary>
	/// <param name="vectorAction">Vector action. Note that for discrete actions, the provided array
	/// will be of length 1.</param>
	/// <param name="textAction">Text action.</param>
	public override void AgentAction(float[] vectorAction, string textAction) {
		vectorAction = normalizeAgentInput(vectorAction);
		FirstPersonControllerAI.InputData inputData;
		inputData.rotHorizontal = vectorAction[0];
		inputData.rotVerticalValue = vectorAction[1];
		inputData.moveForward = vectorAction[2];
		inputData.moveSide = vectorAction[3];
		inputData.fire1 = vectorAction[4] > 0f ? true : false;
		inputData.fire2 = false;
		inputData.jump = false;
		fpc.updateInputAI(inputData);
		AddReward(-1f/agentParameters.maxStep);
	}

// used for debugging
//	void Update () {
//		if (Input.GetMouseButtonDown (0)) {
//			float[] a = {1, 1, 1, 1, 1};
//			this.AgentAction(a, "");
//		}
//
//		if (Input.GetMouseButtonDown (1)) {
//			academy.AcademyReset();
//		}
//	}

	/// <summary>
	/// Normalize NN's result into a valid input for a FPS character controller and avoids instability.
	/// </summary>
	/// <returns>A float between -1 and 1.</returns>
	/// <param name="actionValue">Action value.</param>
	float[] normalizeAgentInput (float[] actionValue) {
		for (int i=0; i<actionValue.Length; i++) {
			actionValue[i] = Mathf.Clamp(actionValue[i], -1f, 1f);
		}
		return actionValue;
	}

	public override void AgentReset() {
		SetReward(1f);
		transform.position = GetRandomSpawnPoint();
		transform.Rotate(new Vector3(0, 1, 0), UnityEngine.Random.Range(1f, 360f));
		arms.transform.rotation = Quaternion.identity;//.Euler(Vector3.zero);
		health.reset();
		fpc.reset();
		grenades = new GameObject[4];
		grenadesBuffer.Clear();
	}

	/// <summary>
	/// Pick a random valid point to spawn Agent.
	/// </summary>
	/// <returns>Spawn point's position.</returns>
	Vector3 GetRandomSpawnPoint () {
		return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)].position;
	}

	/// <summary>
	/// Add grenades inside detector's boundaries as observations
	/// </summary>
	/// <param name="grenade">Grenade.</param>
	public void addObservedGrenade (GameObject grenade) {
		for (int i = 0; i < grenades.Length; i++) {
			if (grenades[i] == null) {
				grenades[i] = grenade;
				return;
			}
		}
		grenadesBuffer.Add(grenade);
		grenadesBuffer.RemoveAll(item => item == null);
	}

	/// <summary>
	/// Removes grenades that go out of the grenade detector's boundaries.
	/// </summary>
	/// <param name="grenade">Grenade.</param>
	public void removeObservedGrenade (GameObject grenade) {
		for (int i = 0; i < grenades.Length; i++) {
			if (grenades[i] == grenade) {
				grenades[i] = null;
				return;
			}
		}
		grenadesBuffer.Remove(grenade);
		grenadesBuffer.RemoveAll(item => item == null);
	}

	/// <summary>
	/// Score a partial goal when Agent damages another agent
	/// </summary>
	public void IScoredAPartialGoal () {
//		addScoreValue("hits");
		AddReward(0.3f);
	}

	/// <summary>
	/// Called when Agent kills another agent
	/// </summary>
	public void IScoredAGoal () {
		if (academy.enableScoreboard) {
			addScoreValue("kills");
		}
		AddReward(10f);
		//Done(); // Mark the agent done which will call AgentReset() automatically
	}

	/// <summary>
	/// Penalize Agent when it damages itself
	/// </summary>
	public void IFailedAPartialGoal () {
//		addScoreValue("self");
		AddReward(-0.2f);
	}

	/// <summary>
	/// Called when Agent dies
	/// </summary>
	public void IFailed () {
		if (academy.enableScoreboard) {
			addScoreValue("deaths");
		}
		AddReward(-10f);
		Done(); // Mark the agent done which will call AgentReset() automatically
	}

	void addScoreValue (string key) {
		if (Application.isEditor && scoreManager != null) {
			scoreManager.addValue(gameObject.name, key);
		}
	}
}

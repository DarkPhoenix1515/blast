using UnityEngine;
using System.Collections.Generic;

public class BlastAcademy : Academy {
	public int agentsPerArena = 4;
	public GameObject agentPrefab;
	public bool enableScoreboard = false;
	List<GameObject> agents = new List<GameObject>();
	ScoreManager scoreManager;

	public override void InitializeAcademy() {
		if (enableScoreboard) {
			scoreManager = GameObject.Find("GameManager").GetComponent<ScoreManager>();
		}
		spawnAgents();
	}

	public override void AcademyReset() {
		foreach (GameObject a in agents) {
			a.GetComponent<BlastAgent>().AgentReset();
		}
	}

	void spawnAgents () {
		GameObject[] arenas = GameObject.FindGameObjectsWithTag("arena");
		int j = 1;

		foreach(GameObject arena in arenas) {
			for (int i=0; i<agentsPerArena; i++) {
				GameObject agent = Instantiate(agentPrefab, Vector3.zero, Quaternion.identity, arena.transform);
				agent.name = "Agent_"+j;
				agents.Add(agent);
				j++;

				if (enableScoreboard) {
					scoreManager.addAgent(agent.name);
				}
			}
		}
	}
}

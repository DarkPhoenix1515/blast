using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ScoreManager : MonoBehaviour {
	// Name Kills Deaths Hits Self-Hits Reward?
	Dictionary<string, Dictionary<string, int>> agentScores;
	public GameObject agentPanel;
	public GameObject agentTextPrefab;
	List<GameObject> textList;

	void Awake () {
		initialize();
	}

	void initialize () {
		if (agentScores != null)
			return;
		textList = new List<GameObject>();
		agentScores = new Dictionary<string, Dictionary<string, int>>();
	}

	public void addAgent (string name) {
		initialize();

		if (agentScores.ContainsKey(name) == false) {
			agentScores[name] = new Dictionary<string, int>();
			agentScores[name]["kills"] = 0;
			agentScores[name]["deaths"] = 0;
			agentScores[name]["hits"] = 0;
			agentScores[name]["self"] = 0;
			GameObject at = Instantiate(agentTextPrefab, agentPanel.transform);
			at.GetComponent<Text>().text = name + "  0  0";
			textList.Add(at);
		}
	}

	void refreshUI () {
		string[] agentNames = getAgents();
		int i = 0;
		foreach(string name in agentNames) {
			textList[i].GetComponent<Text>().text = name + "  " + getScore(name, "kills") + "  " + getScore(name, "deaths");
			i++;
		}
	}

	public void addValue (string name, string key) {
		if (agentScores.ContainsKey(name) == false) {
			return;
		}

		if (agentScores[name].ContainsKey(key) == false) {
			return;
		}
		
		agentScores[name][key] += 1;
		refreshUI();
	}

	public int getScore (string name, string key) {
		if (agentScores.ContainsKey(name) == false) {
			return 0;
		}

		if (agentScores[name].ContainsKey(key) == false) {
			return 0;
		}
		return agentScores[name][key];
	}

	string[] getAgents () {
		return agentScores.Keys.OrderByDescending( a => getScore(a, "kills")).ToArray();
	}
}

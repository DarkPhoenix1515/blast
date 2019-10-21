using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour {
  public static int currentTeam;

  void Start() {
    currentTeam = 1;
  }

  public void switchTeam(Dropdown change) {
    Debug.Log(change.value);
    currentTeam = change.value + 1;
  }

}

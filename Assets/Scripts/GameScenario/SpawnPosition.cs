using UnityEngine;

public class SpawnPosition : MonoBehaviour {

	public Material[] materials;
	private int colorIndex;
  
	void Start() {
    this.colorIndex = 0;
	}

	void OnMouseDown() {
    switchColor();
	}

  void switchColor() {
    this.colorIndex = this.colorIndex != TeamManager.currentTeam ? TeamManager.currentTeam : 0;
    gameObject.GetComponent<Renderer>().material = this.materials[this.colorIndex];
  }
}

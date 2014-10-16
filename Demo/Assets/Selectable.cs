using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {
	
	void Start() {
		renderer.material.color = Color.white;
	}
	
	void OnRTSSelect() {
		renderer.material.color = Color.green;
	}
	
}

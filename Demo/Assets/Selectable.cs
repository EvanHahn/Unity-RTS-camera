using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {

	void Start() {
		renderer.material.color = Color.red;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "RTS Selection") {
			renderer.material.color = Color.green;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.name == "RTS Selection") {
			renderer.material.color = Color.red;
		}
	}

}

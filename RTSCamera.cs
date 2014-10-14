using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour {

	private readonly string INPUT_MOUSE_X = "Mouse X";
	private readonly string INPUT_MOUSE_Y = "Mouse Y";
	private readonly string[] INPUT_MOUSE_BUTTONS = {"Mouse Look", "Mouse Select"};
	
	private bool[] isDragging = new bool[2];
	private Vector2[] dragOrigins = new Vector2[2];
	private Vector3 lookOrigin;

	void Update() {

		for (int index = 0; index <= 1; index ++) {
			if (isClicking(index) && !isDragging[index]) {
				isDragging[index] = true;
				dragOrigins[index] = getMouseCoordinates();
				if (index == 0) { lookOrigin = transform.position; }
			} else if (!isClicking(index) && isDragging[index]) {
				isDragging[index] = false;
			}
		}

		updateLook();
		updateSelect();

	}

	private void updateLook() {
		if (!isDragging[0]) { return; }
		var origin = dragOrigins[0];
		var newPosition = transform.position;
		var mousePosition = getMouseCoordinates();
		newPosition.x = lookOrigin.x + (origin.x - mousePosition.x);
		newPosition.y = lookOrigin.y + (origin.y - mousePosition.y);
		transform.position = newPosition;
	}

	private void updateSelect() {
		if (!isDragging[1]) { return; }
	}

	private bool isClicking(int index) {
		return Input.GetAxis(INPUT_MOUSE_BUTTONS[index]) == 1;
	}

	private Vector2 getMouseCoordinates() {
		return new Vector2(Input.GetAxis(INPUT_MOUSE_X), Input.GetAxis(INPUT_MOUSE_Y));
	}

}

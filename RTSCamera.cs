using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour {
	
	private readonly string[] INPUT_MOUSE_BUTTONS = {"Mouse Look", "Mouse Select"};
	
	private bool[] isDragging = new bool[2];
	private Vector3 selectStartPosition;
	private Texture2D pixel;

	void Start() {
		setPixel(Color.green);
	}
	
	void Update() {
		updateDragging();
		updateLook();
	}

	void OnGUI() {
		updateSelect();
	}

	private void updateDragging() {
		for (int index = 0; index <= 1; index ++) {
			if (isClicking(index) && !isDragging[index]) {
				isDragging[index] = true;
				if (index == 1) {
					selectStartPosition = Input.mousePosition;
				}
			} else if (!isClicking(index) && isDragging[index]) {
				isDragging[index] = false;
			}
		}
	}

	private void updateLook() {
		if (!isDragging[0]) { return; }
		var newPosition = transform.position;
		var mousePosition = getMouseMovement();
		newPosition.x = newPosition.x - mousePosition.x;
		newPosition.y = newPosition.y - mousePosition.y;
		transform.position = newPosition;
	}

	private void updateSelect() {
		if (!isDragging[1]) { return; }
		drawRectangle(selectStartPosition, Input.mousePosition);
	}

	private bool isClicking(int index) {
		return Input.GetAxis(INPUT_MOUSE_BUTTONS[index]) == 1;
	}

	private Vector2 getMouseMovement() {
		return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
	}

	private void drawRectangle(Vector3 cornerA, Vector3 cornerB) {
		var x = cornerA.x;
		var y = cornerA.y;
		var width = cornerB.x - cornerA.x;
		var height = cornerB.y - cornerA.y;
		GUI.DrawTexture(new Rect(x, y, width, 1), pixel);
		GUI.DrawTexture(new Rect(x, y, 1, height), pixel);
		GUI.DrawTexture(new Rect(x, y + height, width, 1), pixel);
		GUI.DrawTexture(new Rect(x + width, y, 1, height), pixel);
	}

	private void setPixel(Color color) {
		pixel = new Texture2D(1, 1);
		pixel.SetPixel(0, 0, color);
		pixel.Apply();
	}

}

using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour {

	public bool disablePanning = false;
	public bool disableSelect = false;
	public Color selectColor = Color.green;
	public float selectLineWidth = 2f;
	
	private readonly string[] INPUT_MOUSE_BUTTONS = {"Mouse Look", "Mouse Select"};
	
	private bool[] isDragging = new bool[2];
	private Vector3 selectStartPosition;
	private Texture2D pixel;

	void Start() {
		setPixel(selectColor);
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
					selectStartPosition = getMousePosition();
				}
			} else if (!isClicking(index) && isDragging[index]) {
				isDragging[index] = false;
			}
		}
	}

	private void updateLook() {
		if (!isDragging[0] || disablePanning) { return; }
		var newPosition = transform.position;
		var mousePosition = getMouseMovement();
		newPosition.x = newPosition.x - mousePosition.x;
		newPosition.y = newPosition.y - mousePosition.y;
		transform.position = newPosition;
	}

	private void updateSelect() {
		if (!isDragging[1] || disableSelect) { return; }
		var x = selectStartPosition.x;
		var y = selectStartPosition.y;
		var width = getMousePosition().x - selectStartPosition.x;
		var height = getMousePosition().y - selectStartPosition.y;
		GUI.DrawTexture(new Rect(x, y, width, selectLineWidth), pixel);
		GUI.DrawTexture(new Rect(x, y, selectLineWidth, height), pixel);
		GUI.DrawTexture(new Rect(x, y + height, width, selectLineWidth), pixel);
		GUI.DrawTexture(new Rect(x + width, y, selectLineWidth, height), pixel);
	}

	private bool isClicking(int index) {
		return Input.GetAxis(INPUT_MOUSE_BUTTONS[index]) == 1;
	}

	private Vector2 getMouseMovement() {
		return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
	}

	private void setPixel(Color color) {
		pixel = new Texture2D(1, 1);
		pixel.SetPixel(0, 0, color);
		pixel.Apply();
	}

	private Vector3 getMousePosition() {
		var result = Input.mousePosition;
		result.y = Screen.height - result.y;
		return result;
	}

}

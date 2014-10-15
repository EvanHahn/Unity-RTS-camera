using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour {

	public bool disablePanning = false;
	public bool disableSelect = false;
	public bool disableZoom = false;

	public float maximumZoom = 1f;
	public float minimumZoom = 20f;
	public Color selectColor = Color.green;
	public float selectLineWidth = 2f;

	public float lookDamper = 5f;
	
	private readonly string[] INPUT_MOUSE_BUTTONS = {"Mouse Look", "Mouse Select"};
	
	private bool[] isDragging = new bool[2];
	private Vector3 selectStartPosition;
	private Texture2D pixel;

	void Start() {
		if (!camera) {
			throw new MissingComponentException("RTS Camera must be attached to a camera.");
		}
		setPixel(selectColor);
	}
	
	void Update() {
		updateDragging();
		updateLook();
		updateZoom();
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
		newPosition.x = newPosition.x - (mousePosition.x * camera.orthographicSize / lookDamper);
		newPosition.y = newPosition.y - (mousePosition.y * camera.orthographicSize / lookDamper);
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

	private void updateZoom() {
		if (disableZoom) { return; }
		var newSize = camera.orthographicSize - Input.GetAxis("Mouse ScrollWheel");
		newSize = Mathf.Clamp(newSize, maximumZoom, minimumZoom);
		camera.orthographicSize = newSize;
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

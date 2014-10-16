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
	public string selectionObjectName = "RTS Selection";
	
	private readonly string[] INPUT_MOUSE_BUTTONS = {"Mouse Look", "Mouse Select"};
	private bool ready;
	private bool[] isDragging = new bool[2];
	private Vector3 selectStartPosition;
	private Texture2D pixel;
	private System.Collections.IList<Object> selectionsToDestroy;

	void Start() {
		try {
			startupChecks();
			ready = true;
		} catch (UnityException exception) {
			ready = false;
			throw exception;
		}
		setPixel(selectColor);
		selectionsToDestroy = new System.Collections.ArrayList<Object>();
	}
	
	void Update() {
		if (!ready) { return; }
		updateDragging();
		updateLook();
		updateZoom();
	}

	void OnGUI() {
		if (!ready) { return; }
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
				if (index == 1) {
					dropSelection(selectStartPosition, getMousePosition());
				}
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
		return Input.GetAxis(INPUT_MOUSE_BUTTONS[index]) == 1f;
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

	private void startupChecks() {
		if (!camera) {
			throw new MissingComponentException("RTS Camera must be attached to a camera.");
		}
		try {
			Input.GetAxis(INPUT_MOUSE_BUTTONS[0]);
			Input.GetAxis(INPUT_MOUSE_BUTTONS[1]);
		} catch (UnityException) {
			throw new UnassignedReferenceException("Inputs " + INPUT_MOUSE_BUTTONS[0] + " and " +
			                                       INPUT_MOUSE_BUTTONS[1] + " must be defined.");
		}
	}

	private void dropSelection(Vector3 screenStart, Vector3 screenEnd) {
		var newSelection = new GameObject(selectionObjectName);
		var collider = newSelection.AddComponent<BoxCollider>() as BoxCollider;
		var start = camera.ScreenToWorldPoint(screenStart);
		var finish = camera.ScreenToViewportPoint(screenEnd);
		var selectionPosition = new Vector3(Mathf.Min(start.x, finish.x),
		                                    Mathf.Min(start.y, finish.y),
		                                    0.5f);
		collider.size = new Vector3(Mathf.Max(start.x, finish.x) - selectionPosition.x,
		                            Mathf.Max(start.y, finish.y) - selectionPosition.y,
		                            0.1f);
		var selection = Instantiate(newSelection, selectionPosition, Quaternion.identity);
		selectionsToDestroy.Add(selection);
		Invoke("destroySelections", 0.1f);
	}

	private void destroySelections() {
		foreach (var selection in selectionsToDestroy) {
			Destroy(selection);
		}
	}

}

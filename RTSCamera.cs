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
	private GameObject selection;
	private bool debugSelection;

	void Start() {
		try {
			startupChecks();
			ready = true;
		} catch (UnityException exception) {
			ready = false;
			throw exception;
		}
		setPixel(selectColor);
		debugSelection = true;
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
					selectStartPosition = Input.mousePosition;
				}
			} else if (!isClicking(index) && isDragging[index]) {
				isDragging[index] = false;
				if (index == 1) {
					dropSelection(selectStartPosition, Input.mousePosition);
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
		var y = Screen.height - selectStartPosition.y;
		var width = (Input.mousePosition - selectStartPosition).x;
		var height = (Screen.height - Input.mousePosition.y) - y;
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
		if (!selection && false) {
			selection = new GameObject(selectionObjectName);
			{
				var collider = selection.AddComponent<BoxCollider>() as BoxCollider;
				collider.isTrigger = true;
				var size = collider.size;
				size.z = 1000000f; // super friggin tall
				collider.size = size;
			}
			{
				var body = selection.AddComponent<Rigidbody>() as Rigidbody;
				body.useGravity = false;
			}
		}
		{
			var start = camera.ScreenToWorldPoint(screenStart);
			start.z = 0;
			var finish = camera.ScreenToWorldPoint(screenEnd);
			finish.z = 0;
			{
				var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.position = start;
				cube.transform.localScale = new Vector3(0.2f, 0.2f,0.2f);
			}
			{
				var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.position = finish;
				cube.transform.localScale = new Vector3(0.2f, 0.2f,0.2f);
			}
			/*
			selection.transform.position = new Vector3(
				Mathf.Min(start.x, finish.x),
				Mathf.Min(start.y, finish.y),
				0.5f);
			selection.transform.localScale = new Vector3(
				Mathf.Max(start.x, finish.x) - selection.transform.position.x,
				Mathf.Max(start.y, finish.y) - selection.transform.position.y,
				0.5f);
			*/
		}
	}

}

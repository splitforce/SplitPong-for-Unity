using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	//This is mostly for fun, you can skip it

	private Transform mainTransform;
	private Transform playerTransform;

	private bool isFPS = true;

	private Vector3 offset = new Vector3(-2.6f, 3.5f, 0);

	private Vector3 fpsCamera = new Vector3 (-20.6f, 3.5f, 0);
	private Vector3 fpsRotate = new Vector3(30f, 90f, 0);
	private Vector3 tpsCamera = new Vector3 (-0.6355f, 24f, 2.45f);
	private Vector3 tpsRotate = new Vector3(90, 0, 0);

	void Start () {
		mainTransform = Camera.main.transform;
		playerTransform = GameObject.Find ("PaddlePlayer").transform;
	}

	void Update () {
		if (Input.GetKeyUp(KeyCode.C)) {
			mainTransform.rotation = Quaternion.identity;
			if (isFPS) {
				isFPS = false;
				mainTransform.position = tpsCamera;
				mainTransform.Rotate(tpsRotate);
			} else {
				isFPS = true;
				mainTransform.Rotate(fpsRotate);
			}
		}

		if (isFPS) {
			mainTransform.position = playerTransform.position + offset;
		}
	}
}

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private float speed = 20F;

	private Vector3 initialPosition;

	void Awake() {
		initialPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
			rigidbody.AddForce(new Vector3(0, 0, -speed));
		}
		else if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.A)) {
			rigidbody.AddForce(new Vector3(0, 0, speed));
		}
	}

	public void SetPlayerSpeed(float newSpeed) {
		speed = newSpeed;
	}

	public void Reset() {
		transform.position = initialPosition;
		rigidbody.velocity = Vector3.zero;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AiController : MonoBehaviour {
	
	private float maxSpeed = 5F;
	private float speed = 2F;

	[HideInInspector]
	public Transform ball;

	private Vector3 initialPosition;
	
	void Awake() {
		initialPosition = transform.position;
	}

	void Start() {
		ball = GameState.Instance.ball.gameObject.transform;
	}

	public void SetMaxSpeed(float newMaxSpeed) {
		maxSpeed = newMaxSpeed;
	}

	public void SeSpeed(float newSpeed) {
		speed = newSpeed;
	}

	// Update is called once per frame
	void Update () {
		float destination = ball.position.z;
		float paddleZ = transform.position.z;

		if (Mathf.Abs (paddleZ - destination) < 0.3) {
			return;
		}

		float mod = paddleZ > destination ? -1 : 1;

		if (mod * rigidbody.velocity.z > maxSpeed) {
			return;
		}

		rigidbody.AddForce(new Vector3(0, 0, mod * speed));
	}
	
	public void Reset() {
		transform.position = initialPosition;
		rigidbody.velocity = Vector3.zero;
	}
}

using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

	[HideInInspector]
	public float startingSpeed = 10.0F;
	[HideInInspector]
	public float acceleration = 1F;
	[HideInInspector]
	public float maxAcceleration = 6F;

	private float currentAcc;

	private Vector3 initialPosition;

	void Awake() {
		initialPosition = transform.position;
	}

	void Start () {
		Reset ();
	}

	public void Reset() {
		transform.position = initialPosition;
		currentAcc = acceleration;
		rigidbody.velocity = Vector3.right * startingSpeed;
		AddFuzzyZ ();
	}

	void OnCollisionEnter(Collision collision) {
		rigidbody.velocity = -1 * Vector3.Reflect(collision.relativeVelocity, collision.contacts[0].normal);

		AddFuzzyZ ();

		if (currentAcc < maxAcceleration) {
			float modX = rigidbody.velocity.x > 0 ? 1 : -1;
			float modZ = rigidbody.velocity.z > 0 ? 1 : -1;
			rigidbody.velocity += new Vector3(modX * acceleration, 0, modZ * acceleration);
			currentAcc += acceleration;
		}
	}

	private void AddFuzzyZ() {
		if (rigidbody.velocity.z == 0) {
			//add some funny randomness
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, Random.value * 2 - 1);
		}
	}
}

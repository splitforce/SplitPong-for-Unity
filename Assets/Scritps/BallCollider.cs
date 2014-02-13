using UnityEngine;
using System.Collections;

public class BallCollider : MonoBehaviour {
	
	void OnCollisionEnter(Collision collision) {
		GameState.Instance.CheckPoint (collision.gameObject);
	}
}

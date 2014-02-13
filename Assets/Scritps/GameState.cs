using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameState : MonoBehaviour {
	private GameObject playerBorder;
	private GameObject aiBorder;
	private PlayerController player;
	private AiController aiPlayer;

	[HideInInspector]
	public BallController ball;



	public int winCount = 5;
	public int playerScore = 0;
	public int aiScore = 0;

	[HideInInspector]
	public bool isPaused = false;

	//this should be state
	[HideInInspector]
	public bool isInitialised = false;
	[HideInInspector]
	public bool isFinished = false;

	private Dictionary<string, string> defaultConfig = new Dictionary<string, string>() {
		{"aiMaxSpeed", "10"},
		{"aiSpeed", "2"},
		{"playerSpeed", "15"},
	};

	public static GameState Instance;
	void Awake() {

		GameState.Instance = this;
		//quick and dirty :)
		playerBorder = GameObject.Find ("/Borders/Left");
		aiBorder = GameObject.Find ("/Borders/Right");
		player = GameObject.Find ("PaddlePlayer").GetComponent<PlayerController> ();
		ball = GameObject.Find ("Ball").GetComponent<BallController> ();
		aiPlayer = GameObject.Find ("PaddleAI").GetComponent<AiController> ();
		PauseGame ();
	}

	void Start() {
		StartCoroutine (InitialiseAssets());
	}

	void Update() {
		if (isPaused && Input.GetKeyUp (KeyCode.Space) && isInitialised) {
			if (isFinished) {
				aiScore = playerScore = 0;
				isFinished = false;
			}
			UnpauseGame();
		}
	}

	private IEnumerator InitialiseAssets() {
		//here we do all pregame things, load assets
		//init object pools etc
		//simulate delay
		float pauseEndTime = Time.realtimeSinceStartup + 1;
		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			yield return 0;
		}

		float aiMaxSpeed = Convert.ToSingle(defaultConfig ["aiMaxSpeed"]);
		float aiSpeed = Convert.ToSingle(defaultConfig ["aiSpeed"]);
		float playerSpeed = Convert.ToSingle(defaultConfig ["playerSpeed"]);

		aiPlayer.SetMaxSpeed (aiMaxSpeed);
		aiPlayer.SeSpeed (aiSpeed);
		player.SetPlayerSpeed (playerSpeed);
		isInitialised = true;
	}


	public void PauseGame() {
		isPaused = true;
		Time.timeScale = 0f;
	}

	public void UnpauseGame() {
		isPaused = false;
		Time.timeScale = 1f;
	}

	public void CheckPoint(GameObject border) {
		if (border != this.playerBorder &&
			border != this.aiBorder) {
		
			return;
		}

		if (border == this.playerBorder) {
			aiScore++;

		} else if (border == this.aiBorder) {
			playerScore++;
		}

		if (aiScore >= winCount) {
			isFinished = true;
		} else if (playerScore >= winCount) {
			isFinished = true;
		}

		ResetGameboard();
		PauseGame ();
	}

	private void ResetGameboard() {
		player.Reset ();
		aiPlayer.Reset ();
		ball.Reset ();
	}
}

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
		{"ballInitialVelocity","750"},
		{"paddleWidth","180"},
		{"ballSize","15"},
	};

	private Dictionary<string, string> config = new Dictionary<string, string> ();

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
		config = defaultConfig;

		UnitySplitForce.SFManager.Instance.initCallback = SplitforceInitialised;
		string appID = "";
		string appKey = "";
		if (string.IsNullOrEmpty(appID) || string.IsNullOrEmpty(appKey)) {
			Debug.LogError("Warning, please put your app id && key before testing sample");
			return;
		}

		UnitySplitForce.SFManager.Init (appID, appKey);
	}

	void Start() {
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

	private void InitialiseAssets() {
		//here we do all pregame things, load assets
		//init object pools etc

		//since we share experiment with iOS, we need to adjust returned values
		float ballInitialSpeed = Convert.ToSingle (config ["ballInitialVelocity"]) / 100f;
		float paddleWidth = Convert.ToSingle (config ["paddleWidth"]) / 24f;
		float ballSize = Convert.ToSingle (config ["ballSize"]) / 3f;
		float aiMaxSpeed = Convert.ToSingle(config ["aiMaxSpeed"]) * 10f;

		float aiSpeed = Convert.ToSingle (config ["aiSpeed"]);



		float playerSpeed = Convert.ToSingle(config ["playerSpeed"]);




		aiPlayer.SetMaxSpeed (aiMaxSpeed);
		aiPlayer.SeSpeed (aiSpeed);
		player.SetPlayerSpeed (playerSpeed);
		ball.SetStartingSpeed (ballInitialSpeed);
		if (ballSize < 11) {
			ball.SetStartingScale (ballSize);
		} else {
			Debug.LogWarning("Unrealistic ball size: " + ballSize);
		}

		player.SetPaddleWidth (paddleWidth);

		ResetGameboard ();
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

		if (isFinished) {
			UnitySplitForce.SFVariation v = UnitySplitForce.SFManager.Instance.getExperiment("GamePhysics");

			if (v != null) {
				v.trackTime("Recency");
				v.endVariation();
			}
		}

		ResetGameboard();
		PauseGame ();
	}

	private void ResetGameboard() {
		player.Reset ();
		aiPlayer.Reset ();
		ball.Reset ();
	}

	void  SplitforceInitialised(bool isFailed, Hashtable additionalData) {
		if (isFailed) {
			if (additionalData.ContainsKey("errorMessage")) {
				Debug.Log("Something wrong: " + additionalData["errorMessage"]);
			}
		}

		UnitySplitForce.SFVariation v = UnitySplitForce.SFManager.Instance.initExperiment("GamePhysics");

		float aiSpeed = Convert.ToSingle(config ["aiSpeed"]);
		float ballInitialVelocity = Convert.ToSingle(config ["ballInitialVelocity"]);
		float paddleWidth = Convert.ToSingle(config ["paddleWidth"]);
		float ballSize = Convert.ToSingle(config ["ballSize"]);

		if (v != null) {
			config["aiSpeed"] = v.VariationData("aiSpeed").DataToFloat(aiSpeed).ToString();
			config["ballInitialVelocity"] = v.VariationData("ballInitialVelocity").DataToFloat(ballInitialVelocity).ToString();
			config["paddleWidth"] = v.VariationData("paddleWidth").DataToFloat(paddleWidth).ToString();
			config["ballSize"] = v.VariationData("ballSize").DataToFloat(ballSize).ToString();
		}

		InitialiseAssets ();
	}
}

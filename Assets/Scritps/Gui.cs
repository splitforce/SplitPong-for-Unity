using UnityEngine;
using System.Collections;

//As of 4.3.1, don't use GUI in production games please, it sucks
public class Gui : MonoBehaviour {
	public GUISkin guiSkin;

	private int areaWidth = 400;
	private int areaHeight = 200;

	private GUIStyle style = new GUIStyle();

	void Awake() {
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f));
		texture.Apply ();
		style.normal.background = texture;
		style.fontSize = 5;

		guiSkin.label.fontSize = (int)(0.08f*Screen.height);
	}

	void OnGUI () {
		GUI.skin = guiSkin;

		if (!GameState.Instance.isInitialised) {
			InitMenu ();
			return;
		}

		if (GameState.Instance.isFinished) {
			FinishedMenu();
			return;
		}

		if (GameState.Instance.isPaused) {
			PausedMenu ();
		}
	}

	private void PausedMenu() {
		GUILayout.BeginArea (new Rect (Screen.width/2 - areaWidth/2,
		                               Screen.height/2 - areaHeight/2,
		                               areaWidth,
		                               areaHeight), style);

		// Begin the singular Horizontal Group
		GUILayout.BeginVertical();

		GUILayout.Label ("Press Space to shot ball");
		GUILayout.Label ("A/D for control");
		GUILayout.EndVertical ();
		GUILayout.EndArea ();

	}

	private void InitMenu() {
		GUILayout.BeginArea (new Rect (Screen.width/2 - areaWidth/2,
		                               Screen.height/2 - areaHeight/2,
		                               areaWidth,
		                               areaHeight), style);

		// Begin the singular Horizontal Group
		GUILayout.BeginVertical();

		GUILayout.Label ("Please wait while game loads");
		GUILayout.Label ("Almost there!");
		GUILayout.EndVertical ();
		GUILayout.EndArea ();
	}

	private void FinishedMenu() {
		GUILayout.BeginArea (new Rect (Screen.width/2 - areaWidth/2,
		                               Screen.height/2 - areaHeight/2,
		                               areaWidth,
		                               areaHeight), style);

		// Begin the singular Horizontal Group
		GUILayout.BeginVertical();
		if (GameState.Instance.aiScore > GameState.Instance.playerScore) {
			GUILayout.Label ("Uh, AI beat you, sorry :O wanna try again?");
		} else {
			GUILayout.Label ("Well done! Do you want to own AI again?");
		}

		GUILayout.Label ("Press Space to reset");
		GUILayout.EndVertical ();
		GUILayout.EndArea ();
	}
}

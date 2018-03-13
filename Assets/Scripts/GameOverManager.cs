using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

	public void PlayAgain() {
		SceneManager.LoadScene("main");
	}

	public void QuitGame() {
		Application.Quit();
	}
}

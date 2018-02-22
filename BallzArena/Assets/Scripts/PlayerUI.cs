using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	private GameObject m_pauseMenu;

	void Start() {
		PauseMenu.m_isOn = false;
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			TogglePauseMenu();
		}
	}

	void TogglePauseMenu() {
		m_pauseMenu.SetActive(!m_pauseMenu.activeSelf);
		PauseMenu.m_isOn = m_pauseMenu.activeSelf;
	}
}

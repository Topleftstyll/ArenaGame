using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

	public Canvas m_networkCanvas;

	[SerializeField]
	private uint m_roomSize = 4;
	private string m_roomName;
	private NetworkManager m_networkManager;

	void Start() {
		m_networkManager = NetworkManager.singleton;
		if(m_networkManager.matchMaker == null) {
			m_networkManager.StartMatchMaker();
		}
	}

	public void SetRoomName(string name) {
		m_roomName = name;
	}

	public void CreateRoom() {
		if(m_roomName != "" && m_roomName != null) {
			m_networkCanvas.gameObject.SetActive(false);
			Debug.Log("Creating Room: " + m_roomName + " with room for " + m_roomSize + " players.");
			// create room
			m_networkManager.matchMaker.CreateMatch(m_roomName, m_roomSize, true, "", "", "", 0, 0, m_networkManager.OnMatchCreate); // change "" and make password variable if you want passwords
		}
	}
}

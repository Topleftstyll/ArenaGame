using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

	public static GameManager Instance { get {return m_instance; }}
	public GameObject Host { get {return m_host;}}

	private static GameManager m_instance;
	private GameObject m_host = null;
	private List<GameObject> m_players = new List<GameObject>();
	[SyncVar]
	private bool isPlaying = true;
	private int m_numPlayersActive = 0;

	public void AddPlayer(GameObject Player) {
		if (m_host == null) {
			m_host = Player;
		}
		if (!m_players.Contains(Player)) {
			m_players.Add(Player);
		}
		m_numPlayersActive++;
	}

	private void Awake() {
		m_instance = this;
	}

	private void Update() {
		Debug.Log(m_players.Count);
		if (!isPlaying) {
			//game is over and we do something here
		}
	}

	public void CheckGameOver() {
		m_numPlayersActive--;
		if(m_numPlayersActive == 1) {
			for(int i = 0; i < m_players.Count; i++) {
				m_players[i].GetComponent<Player>().GameOver();
			}
		}
	}
}

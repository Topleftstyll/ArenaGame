using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour {

	public static GameManager Instance { get {return m_instance; }}
	public GameObject Host { get {return m_host;}}
	public Text m_countDownText;
	public Player m_playerScript;
	public bool m_gameStarted = false;

	private static GameManager m_instance;
	private GameObject m_host = null;
	private List<GameObject> m_players = new List<GameObject>();
	[SyncVar]
	private bool isPlaying = true;
	private bool m_canStartCoroutine = true;
	private int m_numPlayersActive = 0;
	private int m_countDown = 3;

	public void AddPlayer(GameObject Player) {
		if (m_host == null) {
			m_host = Player;
		}
		if (!m_players.Contains(Player)) {
			m_players.Add(Player);
		}
		m_numPlayersActive++;
		m_countDownText.text = "Waiting for players...";
	}

	private void Awake() {
		m_instance = this;
		m_countDownText.text = "";
	}

	void Update() {
		if(m_countDown >= 0 && m_numPlayersActive >= 2) {
			StartGame();
		}
	}

	void StartGame() {
		if(m_canStartCoroutine) {
			m_canStartCoroutine = false;
			StartCoroutine(CountDown());
		}
	}

	public void CheckGameOver() {
		m_numPlayersActive--;
		if(m_numPlayersActive <= 1) {
			for(int i = 0; i < m_players.Count; i++) {
				m_players[i].GetComponent<Player>().GameOver();
			}
		}
	}

	IEnumerator CountDown() {
        yield return new WaitForSeconds(1);
		m_canStartCoroutine = true;
		if(m_countDown > 0) {
			m_countDownText.text = m_countDown.ToString();
			m_countDown--;
		} else {
			m_gameStarted = true;
			m_countDownText.gameObject.SetActive(false);
		}
    }
}

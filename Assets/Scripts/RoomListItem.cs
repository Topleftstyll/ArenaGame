using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour {

	public delegate void JoinRoomDelegate(MatchInfoSnapshot match);
	private JoinRoomDelegate m_joinRoomCallback;

	[SerializeField]
	private Text m_roomNameText;
	private MatchInfoSnapshot m_match;

	public void Setup(MatchInfoSnapshot match, JoinRoomDelegate joinRoomCallback) {
		m_match = match;
		m_joinRoomCallback = joinRoomCallback;
		m_roomNameText.text = m_match.name + " (" + match.currentSize + "/" + match.maxSize + ")";
	}

	public void JoinRoom() {
		m_joinRoomCallback.Invoke(m_match);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

	public Canvas m_networkCanvas;

	[SerializeField]
	private Text m_status;
	[SerializeField]
	public GameObject roomListItemPrefab;
	[SerializeField]
	private Transform m_roomListParent;
	private NetworkManager m_networkManager;
	private List<GameObject> m_roomList = new List<GameObject>();

	void Start() {
		m_networkManager = NetworkManager.singleton;
		if(m_networkManager.matchMaker == null) {
			m_networkManager.StartMatchMaker();
		}

		RefreshRoomList();
	}

	public void RefreshRoomList() {
		ClearRoomList();
		m_networkManager.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnMatchList);
		m_status.text = "Loading...";
	}

	public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)﻿ {
		m_status.text = "";
		if(matchList == null) {
			m_status.text = "Couldn't get room list";
			return;
		}
		ClearRoomList();

		foreach(MatchInfoSnapshot match in matchList) {
			GameObject roomListItemGO = Instantiate(roomListItemPrefab);
			roomListItemGO.transform.SetParent(m_roomListParent);

			RoomListItem roomListItem = roomListItemGO.GetComponent<RoomListItem>();
			if(roomListItem != null) {
				roomListItem.Setup(match, JoinRoom);
			}
			m_roomList.Add(roomListItemGO);
		}

		if(m_roomList.Count == 0) {
			m_status.text = "No rooms at the moment.";
		}
	}

	void ClearRoomList() {
		for(int i = 0; i < m_roomList.Count; i++) {
			Destroy(m_roomList[i]);
		}
		m_roomList.Clear();
	}

	public void JoinRoom(MatchInfoSnapshot match) {
		m_networkManager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, m_networkManager.OnMatchJoined);
		m_status.text = "Joining...";
		m_networkCanvas.gameObject.SetActive(false);
	}
}

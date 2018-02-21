using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLine : MonoBehaviour {

	void OnCollisionEnter(Collision other) {
		if(other.gameObject.name == "Player(Clone)"){
			other.gameObject.GetComponent<Player>().Died();
		} else {
			other.gameObject.SetActive(false);
		}
	}
}

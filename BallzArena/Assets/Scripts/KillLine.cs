using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLine : MonoBehaviour {

	void OnCollisionEnter(Collision other) {
		other.gameObject.SetActive(false);
	}
}

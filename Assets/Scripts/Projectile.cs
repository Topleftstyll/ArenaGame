using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : MonoBehaviour {

    public Collider m_ignoreCollider = null;

	void OnTriggerEnter(Collider other) {
        if(other.tag == "KillLine") {

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public Collider m_ignoreCollider = null;

	void OnTriggerEnter(Collider other) {
        if(other.tag == "KillLine") {
            gameObject.SetActive(false);
        }
    }
}

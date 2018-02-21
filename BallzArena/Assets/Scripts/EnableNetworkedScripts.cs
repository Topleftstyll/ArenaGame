using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnableNetworkedScripts : NetworkBehaviour {

	public RotateWeapon m_rotateWeaponScript;

	void Start() {
		if(isLocalPlayer) {
			GetComponent<Player>().enabled = true;
			m_rotateWeaponScript.enabled = true;
		}
		CameraScript cameraScript = Camera.main.GetComponentInParent<CameraScript>();
		if (!cameraScript.enabled) {
			cameraScript.enabled = true;
		}
		GameManager.Instance.AddPlayer(gameObject);
		cameraScript.m_targets.Add(transform);
	}

	public override void OnStartLocalPlayer() {
        GetComponent<MeshRenderer>().material.color = Color.blue;
		this.tag = "Player";
    }
}

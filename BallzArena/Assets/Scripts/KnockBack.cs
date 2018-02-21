using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class KnockBack : NetworkBehaviour {

	public float m_knockBackAmount = 5.0f;
	public float m_knockBackMultiplier = 1.5f;
	public float m_knockBackWaitTime = 0.5f;
	public Rigidbody m_rb;
	public bool m_isKnocked;

	void OnTriggerEnter(Collider other) {
		if(isLocalPlayer) {
			if(other.gameObject.name == "PlayerBullet(Clone)" && other.tag != "Player") {
				//CmdDestroy(other.gameObject);
				m_isKnocked = true;
				Vector2 dir = other.transform.position - transform.position;
				dir.Normalize();
				//m_rb.AddForce(-dir * m_knockBackAmount, ForceMode.Impulse);
				m_rb.velocity = -dir * m_knockBackAmount;
				m_knockBackAmount *= m_knockBackMultiplier;
				StartCoroutine(IsKnockedBack());
			}
		}
	}

	[Command]
	void CmdDestroy(GameObject other) {
		Destroy(other);
	}

	IEnumerator IsKnockedBack() {
        yield return new WaitForSeconds(m_knockBackWaitTime);
		m_knockBackWaitTime *= m_knockBackMultiplier;
		m_isKnocked = false;
    }
}

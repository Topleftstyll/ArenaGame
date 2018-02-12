using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour {

	public float m_knockBackAmount = 5.0f;
	public float m_knockBackMultiplier = 1.5f;
	public Rigidbody m_rb;
	public bool m_isKnocked;

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.name == "PlayerBullet(Clone)" && other.tag != this.tag) {
			m_isKnocked = true;
			Vector2 dir = other.transform.position - transform.position;
			dir.Normalize();
			m_rb.AddForce(-dir * m_knockBackAmount, ForceMode.Impulse);
			m_knockBackAmount *= m_knockBackMultiplier;
			StartCoroutine(IsKnockedBack());
			Destroy(other.gameObject);
		}
	}

	IEnumerator IsKnockedBack() {
        yield return new WaitForSeconds(.5f);
		m_isKnocked = false;
    }
}

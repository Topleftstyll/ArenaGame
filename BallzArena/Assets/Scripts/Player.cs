using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float m_speed = 10.0f;
	public float m_baseJumpForce = 10.0f;
	public float m_secondThirdJumpForce = 10.0f;

	private Rigidbody m_rb;
	private bool m_isGrounded = false;
	private int m_numOfJumps = 3;
	private int m_currNumOfJumps;

	void Awake() {
		m_rb = GetComponent<Rigidbody>();
		m_currNumOfJumps = m_numOfJumps;
	}

	void Update() {
		if(Input.GetKey(KeyCode.A)) {
			transform.Translate(Vector3.left * Time.deltaTime * m_speed);
		} else if(Input.GetKey(KeyCode.D)) {
			transform.Translate(Vector3.right * Time.deltaTime * m_speed);
		}

		if(Input.GetButtonDown("Jump") && m_currNumOfJumps > 0) {
			if(m_currNumOfJumps == 3) {
				m_rb.AddForce(Vector3.up * m_baseJumpForce, ForceMode.Impulse);
			} else {
				m_rb.AddForce(Vector3.up * m_secondThirdJumpForce, ForceMode.Impulse);
			}
			m_isGrounded = false;
			m_currNumOfJumps--;
		}
		Debug.Log(m_isGrounded);
	}

	void OnCollisionEnter(Collision other) {
		m_isGrounded = true;
		m_currNumOfJumps = m_numOfJumps;
	}
}

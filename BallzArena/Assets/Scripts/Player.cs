﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	public float m_speed = 10.0f;
	public float m_gravityMultiplier = 1.5f;
	public float m_baseJumpForce = 10.0f;
	public float m_bulletSpeed = 6.0f;
	public GameObject m_bulletPrefab;
	public GameObject m_bulletSpawn;

	private Rigidbody m_rb;
	private Vector3 m_gravity;
	private Vector3 m_jumpGravity;
	private bool m_isGrounded = false;
	private int m_numOfJumps = 3;
	private int m_currNumOfJumps;
	private bool m_canShoot = true;
	private KnockBack m_knockBackScript;

	void Awake() {
		m_rb = GetComponent<Rigidbody>();
		m_knockBackScript = GetComponent<KnockBack>();
		m_currNumOfJumps = m_numOfJumps;
		m_gravity = Physics.gravity;
		m_jumpGravity = m_gravity*m_gravityMultiplier;
	}

	void Update() {
		if(!m_knockBackScript.m_isKnocked) {
			if(Input.GetKey(KeyCode.A)) {
				transform.Translate(Vector3.left * Time.deltaTime * m_speed);
			} 

			if(Input.GetKey(KeyCode.D)) {
				transform.Translate(Vector3.right * Time.deltaTime * m_speed);
			}

			if(Input.GetButtonDown("Jump") && m_currNumOfJumps > 0) {
				m_rb.velocity = Vector3.up * m_baseJumpForce;
				Physics.gravity = m_gravity;
				m_isGrounded = false;
				m_currNumOfJumps--;
			}

			if (m_rb.velocity.y < 0.1 && !m_isGrounded) {
				Physics.gravity = m_jumpGravity;
			} else if (m_isGrounded && Physics.gravity == m_jumpGravity) {
				Physics.gravity = m_gravity;
			}
		}

		if(Input.GetMouseButtonDown(0) && m_canShoot) {
			CmdFire();
			m_canShoot = false;
			StartCoroutine(FireRate());
		}
	}

	[Command]
	void CmdFire() {
		Vector2 target = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
		Vector2 direction = target - myPos;
		direction.Normalize();
		Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
		GameObject bullet = PoolManager.Instance.GetObject((int)Objects.Bullet);
		bullet.transform.position = transform.position;
		bullet.transform.rotation = rotation;
		bullet.SetActive(true);
	 	bullet.GetComponent<Rigidbody>().velocity = direction * m_bulletSpeed;
//		NetworkServer.Spawn(bullet);
	}

	[ClientRpc]
	void RpcSetTag(GameObject bullet) {
		bullet.tag = "Player";
	}

	IEnumerator FireRate() {
        yield return new WaitForSeconds(.5f);
		m_canShoot = true;
    }

	void OnCollisionEnter(Collision other) {
		m_isGrounded = true;
		m_currNumOfJumps = m_numOfJumps;
	}
}

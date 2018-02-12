using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float m_speed = 10.0f;
	public float m_baseJumpForce = 10.0f;
	public float m_secondThirdJumpForce = 10.0f;
	public float m_bulletSpeed = 6.0f;
	public GameObject m_bulletPrefab;
	public GameObject m_bulletSpawn;

	private Rigidbody m_rb;
	private bool m_isGrounded = false;
	private int m_numOfJumps = 3;
	private int m_currNumOfJumps;
	private bool m_canShoot = true;

	void Awake() {
		m_rb = GetComponent<Rigidbody>();
		m_currNumOfJumps = m_numOfJumps;
	}

	void Update() {
		if(Input.GetKey(KeyCode.A)) {
			transform.Translate(Vector3.left * Time.deltaTime * m_speed);
		} 

		if(Input.GetKey(KeyCode.D)) {
			transform.Translate(Vector3.right * Time.deltaTime * m_speed);
		}

		if(Input.GetButtonDown("Jump") && m_currNumOfJumps > 0) {
			if(m_rb.velocity.y < 0) {
				m_rb.AddForce(Vector3.up * m_secondThirdJumpForce, ForceMode.Impulse);
			} else {
				m_rb.AddForce(Vector3.up * m_baseJumpForce, ForceMode.Impulse);
			}
			m_isGrounded = false;
			m_currNumOfJumps--;
		}

		if(Input.GetMouseButtonDown(0) && m_canShoot) {
			Fire();
			m_canShoot = false;
			StartCoroutine(FireRate());
		}
	}

	void Fire() {
		Vector2 target = Camera.main.ScreenToWorldPoint( new Vector2(Input.mousePosition.x,  Input.mousePosition.y) );
		Vector2 myPos = new Vector2(m_bulletSpawn.transform.position.x, m_bulletSpawn.transform.position.y);
		Vector2 direction = target - myPos;
		direction.Normalize();
		Quaternion rotation = Quaternion.Euler( 0, 0, Mathf.Atan2 ( direction.y, direction.x ) * Mathf.Rad2Deg );
		GameObject bullet = (GameObject) Instantiate(m_bulletPrefab, myPos, rotation);
	 	bullet.GetComponent<Rigidbody>().velocity = direction * m_bulletSpeed;

		Destroy(bullet, 2.0f);
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

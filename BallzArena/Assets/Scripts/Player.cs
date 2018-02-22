using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour {

	public float m_speed = 10.0f;
	public float m_gravityMultiplier = 1.5f;
	public float m_baseJumpForce = 10.0f;
	public float m_bulletSpeed = 6.0f;
	public float m_knockBackAmount = 5.0f;
	public GameObject m_bulletPrefab;
	public GameObject m_bulletSpawn;

	private Rigidbody m_rb;
	private Vector3 m_gravity;
	private Vector3 m_jumpGravity;
	private bool m_isGrounded = false;
	private int m_numOfJumps = 3;
	private int m_currNumOfJumps;
	private bool m_canShoot = true;
	private bool m_isPowerShot = false;
	private bool m_isKnockedBack = false;
	private bool m_isDead = false;
	private KnockBack m_knockBackScript;
	private float m_powerShotTimer = 0.0f;
	private ParticleSystem m_powerUpParticles;
	private NetworkManager m_networkManager;

	void Awake() {
		m_rb = GetComponent<Rigidbody>();
		m_knockBackScript = GetComponent<KnockBack>();
		m_powerUpParticles = m_bulletSpawn.GetComponent<ParticleSystem>();
		m_powerUpParticles.Stop();
		m_currNumOfJumps = m_numOfJumps;
		m_gravity = Physics.gravity;
		m_jumpGravity = m_gravity*m_gravityMultiplier;
	}

	void Start() {
		m_networkManager = NetworkManager.singleton;
	}

	void Update() {
		if(PauseMenu.m_isOn)
			return;

		if(GameManager.Instance.m_gameStarted) {
			if(!m_knockBackScript.m_isKnocked && !m_isKnockedBack) {
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

			if(Input.GetMouseButton(0)) {
				m_powerShotTimer += Time.deltaTime;
				if(m_powerShotTimer >= 0.1) {
					m_powerUpParticles.Play();
					if(m_powerShotTimer >= 1.0f) {
						CmdChangeParticleColor(Color.red);
						m_isPowerShot = true;
					}
				}
			}

			if(Input.GetMouseButtonUp(0) && m_canShoot) {
				m_powerUpParticles.Stop();
				CmdChangeParticleColor(Color.yellow);
				m_canShoot = false;
				Vector2 target = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
				Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
				Vector2 direction = target - myPos;
				direction.Normalize();
				Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
				if(m_isPowerShot) {
					m_isKnockedBack = true;
					CmdPowerShotFire(rotation, direction);
					m_rb.velocity = -direction * m_knockBackAmount;
					StartCoroutine(DisableMovement());
				} else {
					CmdFire(rotation, direction);
				}
				StartCoroutine(FireRate());
				m_powerShotTimer = 0.0f;
			}
		}
	}

	[Command]
	void CmdFire(Quaternion rotation, Vector2 direction) {
		GameObject bullet = Instantiate(m_bulletPrefab);
		bullet.transform.position = m_bulletSpawn.transform.position;
		bullet.transform.rotation = rotation;
		bullet.GetComponent<Rigidbody>().velocity = direction * m_bulletSpeed;
		NetworkServer.Spawn(bullet);
	}

	[Command]
	void CmdPowerShotFire(Quaternion rotation, Vector2 direction) {
		GameObject bullet = Instantiate(m_bulletPrefab);
		bullet.transform.position = m_bulletSpawn.transform.position;
		bullet.transform.rotation = rotation;
		bullet.GetComponent<Rigidbody>().velocity = direction * m_bulletSpeed;
		NetworkServer.Spawn(bullet);
	}

	[Command]
	void CmdChangeParticleColor(Color color) {
		var main = m_powerUpParticles.main;
		main.startColor = color;
	}

	public void Died() {
		m_isDead = true;
		this.gameObject.SetActive(false);
		GameManager.Instance.CheckGameOver();
	}

	public void GameOver() {
		if(isLocalPlayer){
			MatchInfo matchInfo = m_networkManager.matchInfo;
			m_networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, m_networkManager.OnDropConnection);
			m_networkManager.StopHost();
			if(!m_isDead) {
				// win scene
				SceneManager.LoadScene("Win");
			} else {
				// lose scene
				SceneManager.LoadScene("Lose");
			}
		}
	}

	IEnumerator FireRate() {
        yield return new WaitForSeconds(.5f);
		m_canShoot = true;
    }

	IEnumerator DisableMovement() {
        yield return new WaitForSeconds(.5f);
		m_isKnockedBack = false;
		m_isPowerShot = false;
    }

	void OnCollisionEnter(Collision other) {
		m_isGrounded = true;
		m_currNumOfJumps = m_numOfJumps;
	}
}

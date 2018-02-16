using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public float m_dampTime = 0.2f;
	public float m_screenEdgeBuffer = 12.0f;
	public float m_minSize = 6.5f;
	public List<Transform> m_targets = new List<Transform>();

	private Camera m_camera;
	private float m_zoomSpeed;
	private Vector3 m_moveVelocity;
	private Vector3 m_desiredPosition;

	void Awake() {
		m_camera = GetComponentInChildren<Camera>();
	}

	void FixedUpdate() {
		Move();
		Zoom();
	}

	void Move() {
		FindAveragePosition();
		transform.position = Vector3.SmoothDamp(transform.position, m_desiredPosition, ref m_moveVelocity, m_dampTime);
	}

	void FindAveragePosition() {
		Vector3 closestPlayer = Vector3.zero;
		Vector3 averagePos = Vector3.zero;
		int numtargets = 1;

		for(int i = 0; i < m_targets.Count; i++) {
			if (m_targets[i] == null) {
				m_targets.RemoveAt(i);
			}
			if(m_targets[i].gameObject.activeInHierarchy && m_targets.Count > i) {
				if (m_targets[i].position.x < closestPlayer.x || closestPlayer.x == 0) {
					closestPlayer = m_targets[i].position;
				}
			}
		}
		for(int i = 0; i < m_targets.Count; i++) {
			if(m_targets[i].gameObject.activeInHierarchy) {
				if (m_targets[i].position != closestPlayer) {
					averagePos += m_targets[i].position - closestPlayer;
					numtargets++;
				}
			}
		}
		averagePos /= numtargets;
		m_desiredPosition = closestPlayer + averagePos;
	}

	void Zoom() {
		float requiredSize = FindRequiredSize();	
//		float requiredSize = 20f;
		m_camera.orthographicSize = Mathf.SmoothDamp(m_camera.orthographicSize, requiredSize, ref m_zoomSpeed, m_dampTime);
	}

	float FindRequiredSize() {
		Vector3 desiredLocalPos = transform.InverseTransformPoint(m_desiredPosition);
		float size = 0.0f;

		for(int i = 0; i < m_targets.Count; i++) {
			if(m_targets[i].gameObject.activeSelf) {
				Vector3 targetLocalPos = transform.InverseTransformPoint(m_targets[i].position);
				Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;
				size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
				size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_camera.aspect);
			}
		}
		size += m_screenEdgeBuffer;
		size = Mathf.Max(size, m_minSize);
		return size;
	}
}

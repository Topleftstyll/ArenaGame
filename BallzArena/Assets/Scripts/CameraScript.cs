using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public float m_dampTime = 0.2f;
	public float m_screenEdgeBuffer = 4.0f;
	public float m_minSize = 6.5f;
	public Transform[] m_targets;

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
		Vector3 averagePos = new Vector3();
		int numTargets = 0;

		for(int i = 0; i < m_targets.Length; i++) {
			if(m_targets[i].gameObject.activeSelf) {
				averagePos += m_targets[i].position;
				numTargets++;
			}
		}

		if(numTargets > 0) {
			averagePos /= numTargets;
		}
		averagePos.y = transform.position.y;
		m_desiredPosition = averagePos;
	}

	void Zoom() {
		float requiredSize = FindRequiredSize();
		m_camera.orthographicSize = Mathf.SmoothDamp(m_camera.orthographicSize, requiredSize, ref m_zoomSpeed, m_dampTime);
	}

	float FindRequiredSize() {
		Vector3 desiredLocalPos = transform.InverseTransformPoint(m_desiredPosition);
		float size = 0.0f;

		for(int i = 0; i < m_targets.Length; i++) {
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

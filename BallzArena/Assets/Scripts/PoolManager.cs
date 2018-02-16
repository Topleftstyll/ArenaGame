using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager : MonoBehaviour {

	public enum m_objectTypes {}
	[Serializable]
	public class PooledObject {
		[SerializeField]
		public GameObject m_prefab;
		[SerializeField]
		public int m_numObjects = 10;
		[SerializeField]
		public List<GameObject> m_objects;
	}
	public PooledObject[] m_objectCategory;

	private void Start() {
		
	}

	private void InitPool() {
		for (int i = 0; i < m_objectCategory.Length; i++) {
			
		}
	}
}

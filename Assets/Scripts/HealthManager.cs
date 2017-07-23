using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

	[SerializeField]
	float m_TotalHealth;

	[SerializeField]
	float m_CurrentHealth;

	// Use this for initialization
	void Start () {
		m_CurrentHealth = m_TotalHealth;
		gameObject.tag = "enemy";
	}
	
	// Update is called once per frame
	void Update () {
		if (!isAlive()) {
			Destroy (gameObject);
			return;
		}
	}

	public float takeDamage(float damagePoints){
		m_CurrentHealth -= damagePoints;
		return m_CurrentHealth;
	}

	public bool isAlive(){
		if (m_CurrentHealth <= 0) {
			return false;
		}
		return true;
	}
}

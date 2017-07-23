using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Weapon : MonoBehaviour{


	public string m_Name;

	public int m_TotalRounds = 10; //Clips 
	public int m_TotalAmmoInOneRound = 12;

	public float m_DamagePoints = 10;

	public int m_CurrentRound = 0;
	public int m_CurrentAmmo = 0;

	public float m_FireRate = 0.25f;                                      // Number in seconds which controls how often the player can fire
	public float m_WeaponRange = 500f;                                     // Distance in Unity units over which the player can fire
	public float m_HitForce = 100f;


	public Weapon(string name, float damagePoints){
		m_Name = name;
		m_DamagePoints = damagePoints;
		m_CurrentAmmo = m_TotalAmmoInOneRound;
		m_CurrentRound = m_TotalRounds;
	}

	void Start(){
	
		m_CurrentAmmo = m_TotalAmmoInOneRound;
		m_CurrentRound = m_TotalRounds;
	
	}


	public void shoot(){
		if (m_CurrentRound >= 0 && m_CurrentAmmo > 0) {
			m_CurrentAmmo--;
			if (m_CurrentAmmo <= 0) {
				reload ();
			}
		}
	}

	public void reload(){

		if (m_CurrentRound > 0) {
			m_CurrentRound--;
			m_CurrentAmmo = m_TotalAmmoInOneRound;
		}
	}
}

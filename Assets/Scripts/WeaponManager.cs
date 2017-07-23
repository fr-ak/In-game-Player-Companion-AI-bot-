using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour {


	//[SerializeField]
	//Weapon[] weapons;

	[SerializeField]
	Text text;




	[SerializeField]public Weapon m_CurrentWeapon ;


	// Use this for initialization
	void Awake () {
		m_CurrentWeapon = GetComponent<Weapon> ();		
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "" +
		m_CurrentWeapon.m_CurrentAmmo +
		" / " +
		m_CurrentWeapon.m_TotalAmmoInOneRound +
		"  (" +
		m_CurrentWeapon.m_CurrentRound +
			")";
	}
}

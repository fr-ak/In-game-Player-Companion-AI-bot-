using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {


	[SerializeField] Shooter assultRifle;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.Instance.InputController.Fire1) { //Fire button pressed
			assultRifle.Fire();
		}
	}
}

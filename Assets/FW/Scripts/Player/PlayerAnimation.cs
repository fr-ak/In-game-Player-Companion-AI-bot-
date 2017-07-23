using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerAnimation : MonoBehaviour {


	private Animator animator;


	void Awake () {
		animator = GetComponentInChildren<Animator> ();	
	}
	
	// Update is called once per frame
	void Update () {
		animator.SetFloat ("Vertical", GameManager.Instance.InputController.Vertical);
		animator.SetFloat ("Horizontal", GameManager.Instance.InputController.Horizontal);
	}
}

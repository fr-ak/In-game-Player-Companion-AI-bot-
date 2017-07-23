using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager {

	private GameObject gameObject;

	private static GameManager s_Instance;

	public static GameManager Instance {

		get{ 
			if (s_Instance == null) {
				s_Instance = new GameManager ();
				s_Instance.gameObject = new GameObject ("_gameManager");
				s_Instance.gameObject.AddComponent<InputController> ();
			}
			return s_Instance;
		}
	}

	private InputController s_InputController;

	public InputController InputController {
		get{ 
			if (s_InputController == null) {
				s_Instance.s_InputController = s_Instance.gameObject.GetComponent<InputController> ();
			}
			return s_InputController;
		}
	}
}

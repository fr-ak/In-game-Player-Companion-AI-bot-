using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySpeechToText.Services;


public class BotVoiceController : MonoBehaviour {

	bool m_IsRecording;

	[SerializeField]
	SpeechToTextService m_SpeechToTextService;

	//[SerializeField]
	//CharacterController controller;

	[SerializeField]
	GameObject go_bot;

	private AgentController bot_Controller;

	public GameObject go; //dummy enemy

	private WeaponManager weaponManager;


	public SpeechToTextService SpeechToTextService
	{
		set
		{
			m_SpeechToTextService = value;
			RegisterSpeechToTextServiceCallbacks();
		}
	}

	//Register Callbacks
	public void RegisterSpeechToTextServiceCallbacks(){
		if (m_SpeechToTextService != null)
		{
			m_SpeechToTextService.RegisterOnError(OnSpeechToTextError);
			m_SpeechToTextService.RegisterOnTextResult(OnTextResult);
			m_SpeechToTextService.RegisterOnRecordingTimeout(OnSpeechToTextRecordingTimeout);
		}
	}

	//UnRegister Callbacks
	void UnregisterSpeechToTextServiceCallbacks()
	{
		if (m_SpeechToTextService != null)
		{
			m_SpeechToTextService.UnregisterOnError(OnSpeechToTextError);
			m_SpeechToTextService.UnregisterOnTextResult(OnTextResult);
			m_SpeechToTextService.UnregisterOnRecordingTimeout(OnSpeechToTextRecordingTimeout);
		}
	}

	public void StartRecording()
	{
		Debug.Log("Start service widget recording");

		m_SpeechToTextService.StartRecording();
	}

	public void StopRecording(string comparisonPhrase = null)
	{
		m_SpeechToTextService.StopRecording();
	}




	/**
	Callbacks
	*/
	//Speech to text success
	void OnTextResult(SpeechToTextResult result)
	{
		Debug.Log("Text: " + result.TextAlternatives[0].Text);
		processResult (result.TextAlternatives [0].Text);
	}
	//Timeout error
	void OnSpeechToTextRecordingTimeout()
	{
		Debug.Log(" call timeout");
	}
	//Speech to text error
	void OnSpeechToTextError(string text)
	{
		Debug.Log("error: " + text);
	}



	void OnDestroy()
	{
		UnregisterSpeechToTextServiceCallbacks();
	}

	// Use this for initialization
	void Start () {
		m_IsRecording = false;

		weaponManager = GetComponentInParent<WeaponManager> ();

		bot_Controller = go_bot.GetComponent<AgentController> (); 

		//controller = GetComponentInParent<CharacterController>();

		RegisterSpeechToTextServiceCallbacks();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			if (!m_IsRecording) {
				m_IsRecording = !m_IsRecording;
				StartRecording ();
			}
			Debug.Log ("Pressed left click.");
		}

		if (Input.GetMouseButtonDown (1)) {
			if (m_IsRecording) {
				m_IsRecording = !m_IsRecording;
				StopRecording ();
			}
			Debug.Log ("Pressed right click.");
		}

		if (Input.GetMouseButtonDown(2))
			Debug.Log("Pressed middle click.");
	}


	public void processResult(string result){

		if (result != null && result.Equals ("")) {
			return;
		} else {
			result = result.ToLower ();
			if (result.Contains ("walk") && (result.Contains ("to")) || result.Contains ("two")) {
				Debug.Log ("Walking steps");
				bot_Controller.BotMove (new Vector3 (5, 0, 5));
			} else if (result.Contains ("jump")) {
				Debug.Log ("Jumping");
			} else if ((result.Contains ("shoot") || result.Contains ("short") || result.Contains("fire")
				|| result.Contains("cute") || result.Contains("shirt") || result.Contains("shout")) && result.Contains("enemy")) {
				StartCoroutine( cmd_Shoot ());
			} else if (result.Contains("follow me")){
				Debug.Log ("Following Player");
				bot_Controller.FollowMe ();
			} else if (result.Contains("follow enemy") || result.Contains("photo enemy")){
				
				GameObject[] enemys = GameObject.FindGameObjectsWithTag ("enemy");
				if (enemys.Length > 0) {
					Debug.Log ("Following Enemy");
					bot_Controller.FollowEnemy (enemys [0]);
				} else {
					Debug.Log ("No Enemy");
				}
			} else if (result.Contains("reload")){
				bot_Controller.Reload ();
			} else if ((result.Contains("ammo") || result.Contains("bullets"))
				&& (result.Contains("give") || result.Contains("lend") || result.Contains("gimme") || result.Contains("learn"))){
				weaponManager.m_CurrentWeapon.m_CurrentRound++;
				bot_Controller.Reload ();

			} else if ((result.Contains ("shoot") || result.Contains ("short") || result.Contains ("show") || result.Contains("fire")
				|| result.Contains("cute") || result.Contains("destroy") || result.Contains("sure") || result.Contains("shirt") || result.Contains("shout"))) {

				if (result.Contains ("door") || result.Contains ("gate")) {
					GameObject enemy = GameObject.Find ("_door");
					bot_Controller.Shoot (enemy);
				}

				if (result.Contains("enemy")) {
					StartCoroutine( cmd_Shoot ());
				}

				if (result.Contains ("red") || result.Contains ("trade")) {
					GameObject red = GameObject.FindGameObjectWithTag ("target_red");
					if(red!=null){
						Debug.Log ("shooting targets");
						bot_Controller.Shoot_obj (red);
					}
				}
				if (result.Contains ("blue") || result.Contains ("clue")) {
					GameObject red = GameObject.FindGameObjectWithTag ("target_blue");
					if(red!=null){
						Debug.Log ("shooting targets");
						bot_Controller.Shoot_obj (red);
					}
				}
				if (result.Contains ("green") || result.Contains ("grain") || result.Contains ("crane") || result.Contains ("train")) {
					GameObject red = GameObject.FindGameObjectWithTag ("target_green");
					if(red!=null){
						Debug.Log ("shooting targets");
						bot_Controller.Shoot_obj (red);
					}
				}
				if (result.Contains ("black")) {
					GameObject red = GameObject.FindGameObjectWithTag ("target_black");
					if(red!=null){
						Debug.Log ("shooting targets");
						bot_Controller.Shoot_obj (red);
					}
				}
				if (result.Contains ("white")) {
					GameObject red = GameObject.FindGameObjectWithTag ("target_white");
					if(red!=null){
						Debug.Log ("shooting targets");
						bot_Controller.Shoot_obj (red);
					}
				}

			} 
		}
	}

	IEnumerator cmd_Shoot(){
		GameObject[] enemys = GameObject.FindGameObjectsWithTag ("enemy");

		//System.Diagnostics.Process.Start("say " + "\"Leave it to me. Found "+enemys.Length+" enemys.\"");
	
		System.Diagnostics.Process.Start("/usr/bin/osascript", "-e say \\\"" + "Hello how are you" + "\\\"");

		foreach(GameObject go in enemys){
			yield return new WaitUntil (()=>!bot_Controller.isCoroutineRunning());
			bot_Controller.Shoot (go);
		}
	}
		

}

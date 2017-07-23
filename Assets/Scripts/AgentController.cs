using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class AgentController : MonoBehaviour {

	public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
	public ThirdPersonCharacter character { get; private set; } // the character we are controlling

	WeaponManager m_WeaponManager;

	AICharacterControl m_Controller;

	[SerializeField] private float bot_Move_Speed = 1f;

	[SerializeField] private float fire_rate = 0.05f;

	[SerializeField]
	GameObject damageEffect;

	private bool coroutine_check = false;

	void Awake(){
		//nav_agent = GetComponent<NavMeshAgent> ();
	}

	// Use this for initialization
	void Start () {
		// get the components on the object we need ( should not be null due to require component so no need to check )
		agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
		character = GetComponent<ThirdPersonCharacter>();

		m_WeaponManager = GetComponent<WeaponManager> ();
		m_Controller = GetComponent<AICharacterControl> ();
	}
	
	// Update is called once per frame
	void Update () {

		//nav_agent.SetDestination (mainPlayer.position);


	}

	public void BotMove(Vector3 position){

		agent.Move (position * bot_Move_Speed);

	}

	public void Shoot(GameObject go){
		if (go == null)
			return;
		//GameObject[] enemys = GameObject.FindGameObjectsWithTag ("enemy");
		NavMeshHit hit;
		if (!agent.Raycast (go.transform.position, out hit)) {
			//target is visible
			Debug.Log("Shooting");

			//_Shoot (go, true);
			StartCoroutine(_Shoot_(go,1));
		} else {
			//in visible
			Debug.Log("Cant shoot");
		}
	}

	public void Shoot_obj(GameObject go){
		if (go == null)
			return;
		//GameObject[] enemys = GameObject.FindGameObjectsWithTag ("enemy");
		NavMeshHit hit;
		if (!agent.Raycast (go.transform.position, out hit)) {
			//target is visible
			Debug.Log("Shooting_obj");

			//_Shoot (go, true);
			StartCoroutine(_Shoot_obj(go,hit));
		} else {
			//in visible
			Debug.Log("Can't shoot");
		}
	}


	public void FollowMe(){
		_FollowMe ();
	}

	public void Reload(){
		_Reload ();
	}

	public void FollowEnemy(GameObject g_o){
		_FollowEnemy (g_o.transform);
	}

	private void _Shoot(GameObject go, bool wait){
		//change the destination
		m_Controller.SetEnemyTarget(go.transform);

		//gun effect
		ParticleSystem ps = GetComponentInChildren<ParticleSystem> ();
		ps.Play();
		//shoot effect
		//GameObject _go = Instantiate(damageEffect, hit.position, Quaternion.LookRotation(hit.normal));
		//Destroy (_go, 2f);
		//handel health
		HealthManager enemy = go.GetComponentInParent<HealthManager> ();
		if (enemy.isAlive ()) {
			enemy.takeDamage (m_WeaponManager.m_CurrentWeapon.m_DamagePoints);
			if (enemy != null && enemy.isAlive ()) {
				_Shoot (go,false);
			}
		}

	}

	IEnumerator _Shoot_(GameObject go, float wait){
		coroutine_check = true;
		//change the destination
		m_Controller.SetEnemyTarget(go.transform);
		//wait untill player turn towards enemy

		yield return new WaitForSeconds(wait);

		//gun effect
		ParticleSystem ps = GetComponentInChildren<ParticleSystem> ();
		ps.Play();
		//shoot effect
		//GameObject _go = Instantiate(damageEffect, hit.position, Quaternion.LookRotation(hit.normal));
		//Destroy (_go, 2f);
		//handel health
		HealthManager enemy = go.GetComponentInParent<HealthManager> ();
		if (enemy.isAlive ()) {
			m_WeaponManager.m_CurrentWeapon.shoot ();
			enemy.takeDamage (m_WeaponManager.m_CurrentWeapon.m_DamagePoints);
			if (enemy != null && enemy.isAlive ()) {
				StartCoroutine (_Shoot_ (go, fire_rate));
			} else {
				coroutine_check = false;
				//Follow player again
				m_Controller.SetPlayerTarget();
			}
		}

	}

	IEnumerator _Shoot_obj(GameObject go, NavMeshHit hit){
		coroutine_check = true;
		//change the destination
		m_Controller.SetEnemyTarget(go.transform);
		//wait untill player turn towards enemy

		yield return new WaitForSeconds(1);

		//gun effect
		ParticleSystem ps = GetComponentInChildren<ParticleSystem> ();
		ps.Play();

		//add force
		Rigidbody rb = go.GetComponent<Rigidbody>();
		rb.AddForce (Vector3.forward * 200f);

		//shoot effect
		GameObject _go = Instantiate(damageEffect, hit.position, Quaternion.LookRotation(hit.normal));
		Destroy (_go, 2f);



		//shoot
		m_WeaponManager.m_CurrentWeapon.shoot ();

		//Follow player again
		m_Controller.SetPlayerTarget();


	}

	private void _FollowMe(){
		m_Controller.SetPlayerTarget ();
	}

	private void _FollowEnemy(Transform tr){
		m_Controller.FollowEnemy (tr);
	}

	private void _Reload(){
		m_WeaponManager.m_CurrentWeapon.reload ();
	}

	public bool isCoroutineRunning(){
		return coroutine_check;
	}
}

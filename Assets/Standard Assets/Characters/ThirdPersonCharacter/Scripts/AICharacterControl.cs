using System;
using UnityEngine;
using UnityEngine.AI;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        
		public Transform target;                                    // target to aim for

		private float distance_limit;

		private bool b_Shoot = false;

		private string target_name_voice;




        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;

			//distance_limit = 0;

        }


        private void Update()
        {
			if (target != null)
				agent.SetDestination (target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
        }


		public void SetEnemyTarget (Transform target){
			_SetTarget (target);
			agent.stoppingDistance = _GetDistanceFormTarger (target)-0.8f;
		}

		public void SetPlayerTarget (){
			GameObject go = GameObject.FindGameObjectWithTag ("Player");
			_SetTarget (go.transform);
			agent.stoppingDistance = 2f;
		}

		private void _SetTarget(Transform target)
        {
            this.target = target;
        }

		private float _GetDistanceFormTarger(Transform target){
			return  Vector3.Distance (target.position, transform.position);
		}

		public void FollowEnemy(Transform target){
			_SetTarget (target);
		}
    }
}

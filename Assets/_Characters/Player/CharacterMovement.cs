using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;


namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] float stoppingDistance = 1f;

        ThirdPersonCharacter Character;   // A reference to the ThirdPersonCharacter on the object
        Vector3 clickPoint;
        GameObject walkTarget;
        NavMeshAgent agent;


        void Start()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            Character = GetComponent<ThirdPersonCharacter>();
            walkTarget = new GameObject("walkTarget");

            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updatePosition = true;
            agent.stoppingDistance = stoppingDistance;

            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;  // add function to do to observe onMouseOverEnemy event (when event happenes, tells observer, and observer does this function)
        }

        void Update()
        {
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                Character.Move(agent.desiredVelocity, false, false);
            }
            else
            {
                Character.Move(Vector3.zero, false, false);
            }
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                walkTarget.transform.position = destination;
                agent.SetDestination(destination);
            }
        }

        void OnMouseOverEnemy(Enemy enemy) // pass in enemy
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                // navigate to the enemy
                agent.SetDestination(enemy.transform.position);
            }
        }





        //	void OnDrawGizmos()
        //	{
        //		// Draw movement Gizmos
        //		Gizmos.color = Color.black;
        //		Gizmos.DrawLine (transform.position, clickPoint);
        //		Gizmos.DrawSphere (currentDestination, 0.1f);
        //        Gizmos.DrawSphere(clickPoint, 0.15f);
        //
        //        // Draw attack sphere
        //        Gizmos.color = new Color(255f, 0f, 0, .3f);
        //        Gizmos.DrawWireSphere(transform.position, stopToAttackRadius);
        //        // try out different gizmos
        //    }



    }
}

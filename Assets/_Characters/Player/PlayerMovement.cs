using System;
using UnityEngine;
using UnityEngine.AI;


namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {
        ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
        CameraUI.CameraRaycaster cameraRaycaster = null;
        Vector3 clickPoint;
        GameObject walkTarget = null;


        void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraUI.CameraRaycaster>();
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
            walkTarget = new GameObject("walkTarget");


            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;  // add function to do to observe onMouseOverEnemy event (when event happenes, tells observer, and observer does this function)
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                walkTarget.transform.position = destination;
                // aiCharacterControl.SetTarget(walkTarget.transform);
            }
        }

        void OnMouseOverEnemy(Enemy enemy) // pass in enemy
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                // navigate to the enemy
                // aiCharacterControl.SetTarget(enemy.transform);
            }
        }




        // TODO make this get called again
        private void ProcessDirectMent()
        {
            // read inputs
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // calculate camera relative direction to move:

            Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = v * camForward + h * Camera.main.transform.right;

            thirdPersonCharacter.Move(move, false, false);
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

using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AICharacterControl))]
[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
	ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
	CameraRaycaster cameraRaycaster = null;
	Vector3 currentDestination, clickPoint;
	AICharacterControl aiCharacterControl = null;

    bool isInGamePadMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
		aiCharacterControl = GetComponent<AICharacterControl> ();

		cameraRaycaster.notifyMouseClickObservers +=
    }

	void ProcessMouseClick()
	{
		
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


	void OnDrawGizmos()
	{
		// Draw movement Gizmos
		Gizmos.color = Color.black;
		Gizmos.DrawLine (transform.position, clickPoint);
		Gizmos.DrawSphere (currentDestination, 0.1f);
        Gizmos.DrawSphere(clickPoint, 0.15f);

        // Draw attack sphere
        Gizmos.color = new Color(255f, 0f, 0, .3f);
        Gizmos.DrawWireSphere(transform.position, stopToAttackRadius);
        // try out different gizmos
    }



}


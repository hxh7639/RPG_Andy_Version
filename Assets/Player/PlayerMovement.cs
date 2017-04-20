using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float stopToAttackRadius = 5f;

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
	Vector3 currentDestination, clickPoint;

    bool isInGamePadMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }


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

    //private void ProcessMouseMovement()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        clickPoint = cameraRaycaster.hit.point;
    //        switch (cameraRaycaster.currentLayerHit)
    //        {
    //            case Layer.Walkable:
    //                currentDestination = DestinationStoppingPoint(clickPoint, walkMoveStopRadius);
    //                break;
    //            case Layer.Enemy:
    //                currentDestination = DestinationStoppingPoint(clickPoint, stopToAttackRadius);
    //                break;
    //            default:
    //                Debug.Log("Unexpected Layer found, CLICK TO MOVE ERROR");
    //                return;
    //        }

    //    }
    //    WalkToDestination();
    //}

    private void WalkToDestination()
    {
        var playerToClickPoint = currentDestination - transform.position;
        if (playerToClickPoint.magnitude >= walkMoveStopRadius)
        {
            thirdPersonCharacter.Move(playerToClickPoint, false, false); // if you want to hold click to move, put it inside the if(input) statement above. if you want to click once to move, outside of the if(input) statement.
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    Vector3 DestinationStoppingPoint(Vector3 destinatiion, float shortening)
	{
		Vector3 reductionVector = (destinatiion - transform.position).normalized * shortening;
		return destinatiion - reductionVector;
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


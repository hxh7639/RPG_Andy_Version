using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float walkMoveStopRadius = 0.2f;
	
    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            print("Cursor raycast hit " + cameraRaycaster.layerHit);
            switch (cameraRaycaster.layerHit)
            {
            	case Layer.Walkable:
					currentClickTarget = cameraRaycaster.hit.point; 
					break;
				case Layer.Enemy:
					Debug.Log("not moving to enemy");
					break;
				default:
					Debug.Log("Unexpected Layer found, CLICK TO MOVE ERROR");
					return;
            }

        }
		var playerToClickPoint = currentClickTarget - transform.position;
        if (playerToClickPoint.magnitude >= walkMoveStopRadius)
        {
			m_Character.Move(playerToClickPoint, false, false); // if you want to hold click to move, put it inside the if(input) statement above. if you want to click once to move, outside of the if(input) statement.
		}else
		{
			m_Character.Move(Vector3.zero,false,false);
		}
	}
}


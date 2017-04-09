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

    bool isInGamePadMode = false; // TODO consider making static later

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G)) // G for gamePad. 
            //TODO allow player to map later or add to menu
        {
            isInGamePadMode = !isInGamePadMode; // toggle mode
        }

        if (isInGamePadMode)
        {
            ProcessDirectMent();
        }
        else
        {
            ProcessMouseMovement();
        }
    }

    private void ProcessDirectMent()
    {
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate camera relative direction to move:

        Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 m_Move = v * m_CamForward + h * Camera.main.transform.right;

        m_Character.Move(m_Move, false, false);
    }

    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
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
        }
        else
        {
            m_Character.Move(Vector3.zero, false, false);
        }
    }
}


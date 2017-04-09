using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAffordance : MonoBehaviour {

    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D TargetCursor = null;
    [SerializeField] Texture2D UnknownCursor = null;

	CameraRaycaster cameraRaycaster;
    [SerializeField] Vector2 cursorHotspot = new Vector2 (96,96);

	// Use this for initialization
	void Start () {
		cameraRaycaster = FindObjectOfType<CameraRaycaster>();

	}
	
	// Update is called once per frame
	void LateUpdate () {
		// Debug.Log(cameraRaycaster.layerHit);
        switch (cameraRaycaster.layerHit)
        {
            case Layer.Walkable:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.Enemy:
                Cursor.SetCursor(TargetCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.RaycastEndStop:
                Cursor.SetCursor(UnknownCursor, cursorHotspot, CursorMode.Auto);
                break;

            default:
                Debug.LogError("dont know what cursor to show");

                return;
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {

	CameraRaycaster cameraRaycaster;

	// Use this for initialization
	void Start () {
		cameraRaycaster = FindObjectOfType<CameraRaycaster>();

	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(cameraRaycaster.layerHit);
		
	}
}

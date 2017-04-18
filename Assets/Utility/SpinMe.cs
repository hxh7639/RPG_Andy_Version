using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMe : MonoBehaviour {

	[SerializeField] float xRotationsPerMinute = 1f;
	[SerializeField] float yRotationsPerMinute = 1f;
	[SerializeField] float zRotationsPerMinute = 1f;
	
	void Update () {

		// xDegreesPerFrame = Time.DeltaTime, 60, 360, xRotationPerMinute
		// degrees / Frame = (seconds / frame) , (60 seconds / minute), (360 degrees / rotation), (Rotation / Minute)
		// degrees / Frame = (360 degrees / rotation) * (Rotation / Minute) / (60 seconds / minute) * (seconds / frame)
		// xDegreesPerFrame = 360 * xRotationPerMinute * Time.DeltaTime / 60


        float xDegreesPerFrame = 0; // TODO COMPLETE ME
        transform.RotateAround (transform.position, transform.right, xDegreesPerFrame);

		float yDegreesPerFrame = 0; // TODO COMPLETE ME
        transform.RotateAround (transform.position, transform.up, yDegreesPerFrame);

        float zDegreesPerFrame = 0; // TODO COMPLETE ME
        transform.RotateAround (transform.position, transform.forward, zDegreesPerFrame);
	}
}

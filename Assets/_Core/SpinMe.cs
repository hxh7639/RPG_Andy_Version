using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class SpinMe : MonoBehaviour
    {

        [SerializeField] float xRotationsPerMinute = 1f;
        [SerializeField] float yRotationsPerMinute = 1f;
        [SerializeField] float zRotationsPerMinute = 1f;

        void Update()
        {

            // xDegreesPerFrame = Time.DeltaTime, 60, 360, xRotationPerMinute
            // degrees / Frame = (seconds / frame) , (60 seconds / minute), (360 degrees / rotation), (Rotation / Minute)
            // degrees / Frame = (360 degrees / rotation) * (Rotation / Minute) / (60 seconds / minute) * (seconds / frame)
            // xDegreesPerFrame = 360 * xRotationsPerMinute * Time.deltaTime / 60;


            float xDegreesPerFrame = 360 * xRotationsPerMinute * Time.deltaTime / 60;
            transform.RotateAround(transform.position, transform.right, xDegreesPerFrame);

            float yDegreesPerFrame = 360 * yRotationsPerMinute * Time.deltaTime / 60;
            transform.RotateAround(transform.position, transform.up, yDegreesPerFrame);

            float zDegreesPerFrame = 360 * zRotationsPerMinute * Time.deltaTime / 60;
            transform.RotateAround(transform.position, transform.forward, zDegreesPerFrame);
        }
    }
}
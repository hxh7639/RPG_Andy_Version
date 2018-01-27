using UnityEngine;

namespace RPG.Characters
{
    public class FaceCamera : MonoBehaviour
    {
        Camera cameraToLookAt;
        public float cameraXMultiplier = 2f;
        public float cameraYMultiplier = 10f;
        public float cameraZMultiplier = 2f;

        // Use this for initialization 
        void Start()
        {
            cameraToLookAt = Camera.main;
        }

        // Update is called once per frame 
        void LateUpdate()
        {
            var cameraExtendedX = cameraToLookAt.transform.position.x * cameraXMultiplier;
            var cameraExtendedY = cameraToLookAt.transform.position.y * cameraYMultiplier;
            var cameraExtendedZ = cameraToLookAt.transform.position.z * cameraZMultiplier;
            Vector3 cameraExtended = new Vector3(cameraExtendedX, cameraExtendedY, cameraExtendedZ);
            transform.LookAt(cameraExtended);
        }
    }
}
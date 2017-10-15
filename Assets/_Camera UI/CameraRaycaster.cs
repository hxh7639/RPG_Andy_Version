using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using RPG.Characters;  // so we can detect by type (knowing if it is enemy)

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        const int POTENTIALLY_WALKABLE_LAYER = 8;
        float maxRaycastDepth = 300f; // Hard coded value, Course used 100f

        Rect currentScreenRect = new Rect();

        public delegate void OnMouseOverEnemy(Enemy enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;

        public delegate void OnMouseOverTerrian(Vector3 destination);
        public event OnMouseOverTerrian onMouseOverPotentiallyWalkable;
        
 

        void Update()
        {
            currentScreenRect = new Rect(0, 0, Screen.width, Screen.height);
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Impliment UI interaction
            }
            else
            {
                PerformRaycasts();
            }
        }

        void PerformRaycasts()
        {
            if (currentScreenRect.Contains(Input.mousePosition))  // if mouse is within the screenRect area, then Ray Cast
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // Specify layer priorities here, order matters
                if (RaycastForEnemy(ray)) { return; }
                if (RaycastForPotentiallyWalkable(ray)) { return; }
            }
        }


        bool RaycastForEnemy(Ray ray)
        {
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            var gameObjectHit = hitInfo.collider.gameObject; // looking at hitinfo and returns the gameObject it hits
            var enemyHit = gameObjectHit.GetComponent<Enemy>(); // enemyHit is the object hit with "Enemy" component on it
            if (enemyHit) // if there is such thing (object hit with an "Enemy" component on it, do this
            {
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto); // change mouse cursor
                onMouseOverEnemy(enemyHit); // pass in the enemyHit and tell it's event observers about it
                return true;
            }
            return false;
        }

        private bool RaycastForPotentiallyWalkable(Ray ray)
        {
            RaycastHit hitInfo;
            LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
            bool potentiallyWalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potentiallyWalkableLayer); // see if we hit something that's potentially walkable, only returns true if we do
            if (potentiallyWalkableHit)
            {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverPotentiallyWalkable(hitInfo.point);
                return true;
            }
            return false;
        }
    }
}
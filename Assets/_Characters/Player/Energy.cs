using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] RawImage energyBarImage;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] int energyToSpend = 10;

        CameraRaycaster cameraRaycaster;
        float currentEnergyPoints;
        
        // Use this for initialization
        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.notifyRightClickObservers += ProcessRightClick; // Register right mouse click
        }

        void ProcessRightClick(RaycastHit raycastHit, int layerHit)
        {
            SpendEnergy(energyToSpend);
            print(currentEnergyPoints);
            UpdateEnergyBar();
        }

        public void SpendEnergy(float energySpentPerUse)
        {
            float newEnergyPoints = currentEnergyPoints - energySpentPerUse;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0f, maxEnergyPoints);
        }

        private void UpdateEnergyBar()
        {
            float xValue = -(EnergyAsPercentage() / 2f) - 0.5f;
            energyBarImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        float EnergyAsPercentage()
        {
                return currentEnergyPoints / maxEnergyPoints;
        }
    }
}

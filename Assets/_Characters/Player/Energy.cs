using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] RawImage energyBarImage = null;
        [SerializeField] float maxEnergyPoints = 100f;

        CameraUI.CameraRaycaster cameraRaycaster;
        float currentEnergyPoints;
        
        // Use this for initialization
        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;

        }

        public bool IsEnergyAvailable(float amount)
        {
            return amount <= currentEnergyPoints;
        }

        public void ConsumeEnergy(float amount)
        {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0f, maxEnergyPoints);
            UpdateEnergyBar();
        }

        private void UpdateEnergyBar()
        {
            // TODO remove magic numbers
            float xValue = -(EnergyAsPercentage() / 2f) - 0.5f;
            energyBarImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        float EnergyAsPercentage()
        {
                return currentEnergyPoints / maxEnergyPoints;
        }
    }
}

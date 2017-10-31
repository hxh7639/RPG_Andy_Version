using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class SpecialAbilities : MonoBehaviour
    {
        // Temporarily serialized for debugging
        [SerializeField] AbilityConfig[] abilities;

        [SerializeField] Image energyBarImage;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 1f;
        // TODO add outOfEnergy (sound);

        float currentEnergyPoints;
        AudioSource audioSource;

        float energyAsPercentage { get { return currentEnergyPoints / maxEnergyPoints; } } // this is a getter, or you can make a method "EnergyAsPercentage()"


        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            currentEnergyPoints = maxEnergyPoints;
            AttachInitialAbilities();
            UpdateEnergyBar();
        }

        void Update()
        {
            if (currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergyPoint();
                UpdateEnergyBar();
            }
        }

        void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
                abilities[abilityIndex].AttachAbilityTo(gameObject);
        }

        public void AttempSpecialAbility(int abilityIndex)
        {
            var energyComponent = GetComponent<SpecialAbilities>();
            var energyCost = abilities[abilityIndex].GetEnergyCost();  // to make it read from scripttible object

            if (energyCost <= currentEnergyPoints)
            {
                ConsumeEnergy(energyCost);
                print("Using special ability " + abilityIndex); // TODO make it work
            }
            else
            {
                //TODO Play out of energy sound
            }

        }

        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }

        private void AddEnergyPoint()
        {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;  // multiply Time.deltaTime to make things happen per second instead of per frame.
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints); // if you don't clamp it it may go above MAX if regends too fast.
        }


        public void ConsumeEnergy(float amount)
        {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0f, maxEnergyPoints);
            UpdateEnergyBar();
        }

        private void UpdateEnergyBar()
        {
            energyBarImage.fillAmount = energyAsPercentage;
        }


    }
}

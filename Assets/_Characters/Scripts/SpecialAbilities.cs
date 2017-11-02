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
        [SerializeField] AudioClip outOfEnergy;

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

        public void AttempSpecialAbility(int abilityIndex, GameObject target = null) // by setting target = null, the whole arguement becomes optional.
        {
            var energyComponent = GetComponent<SpecialAbilities>();
            var energyCost = abilities[abilityIndex].GetEnergyCost();  // reads from scripttible object

            if (energyCost <= currentEnergyPoints)
            {
                ConsumeEnergy(energyCost);
                abilities[abilityIndex].Use(target); // because the target is set to null, it will only call the target if there is one. otherwise it will just call nothing and still cast abilities.
            }
            else
            {
                audioSource.clip = outOfEnergy;
                audioSource.Play();
                // lecture uses audioSource.PlayOneShot(outOfEnergy); // always audioSource.playOneShot but put the audio you want to play in ()
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

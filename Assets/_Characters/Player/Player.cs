using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
// TODO consider re-wiring, don't want Player script to be affected by CameraUI, core,and weapons
using RPG.CameraUI; 
using RPG.Core; 
using RPG.Weapons; 

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {

        [SerializeField] float maxHealthPoints = 100;
        [SerializeField] float baseDamage = 11f;
        [SerializeField] Weapon weaponInUse = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;

        // Temporarily serialized for dugging
        [SerializeField] SpecialAbility[] abilities;

        Animator animator;
        float currentHealthPoints;
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0f;


        public float healthAsPercentage
        {
            get
            {
                return currentHealthPoints / maxHealthPoints;
            }
        }

        void Start()
        {
            RegisterForMouseClick();
            SetCurrentMaxHealth();
            SpawnWeaponInHand();
            SetupRuntimeAnimator();
            abilities[0].AttachComponentTo(gameObject);
        }

        public void TakeDamage(float Damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - Damage, 0f, maxHealthPoints);
        }

        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        private void SetupRuntimeAnimator() 
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip(); //TODO remove const
        }

        private void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy; // adding OnMouseOverEnemy to event list for observing onMouseOverEnemy

        }

        void OnMouseOverEnemy(Enemy enemy) // observing onMouseOverEnemy, when it happenes it passes (Enemy enemy) to this method
        {
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject)) // enemy.gameobject selects the parent gameObject for the enemy script
            {
                AttackTarget(enemy);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                AttempSpecialAbility(0, enemy);
            }
        }

        private void AttempSpecialAbility(int abilityIndex, Enemy enemy)
        {
            var energyComponent = GetComponent<Energy>();
            var energyCost = abilities[abilityIndex].GetEnergyCost();

            if (energyComponent.IsEnergyAvailable(energyCost)) // TODO read from Scripttible Object
            {
                energyComponent.ConsumeEnergy(energyCost);
                var abilityParams = new AbilityUseParams(enemy, baseDamage);
                abilities[abilityIndex].Use(abilityParams);
                // TODO Use the ability
            }

        }

        private void AttackTarget(Enemy enemy)
        {
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())
            {
                animator.SetTrigger("Attack"); // TODO make const
                enemy.TakeDamage(baseDamage);
                lastHitTime = Time.time;
            }
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();
        }

        void SpawnWeaponInHand()
        {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject mainHand = RequestMainHand();
            var weapon = Instantiate(weaponPrefab, mainHand.transform);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;

        }

        private GameObject RequestMainHand()  // find main hand
        {
            var mainHands = GetComponentsInChildren<MainHand>();
            int numberOfMainHands = mainHands.Length;

            Assert.IsFalse(numberOfMainHands <= 0, "No MainHand found on player, please add one"); // handle 0 hand
            Assert.IsFalse(numberOfMainHands > 1, "Multiple MainHand Scripts on Player, please remove the extra ones"); //handle 1 hand

            return mainHands[0].gameObject;

        }
    }
}
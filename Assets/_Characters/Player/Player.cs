using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
// TODO consider re-wiring, don't want Player script to be affected by CameraUI, core,and weapons
using RPG.CameraUI; 
using RPG.Core; 
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class Player : MonoBehaviour
    {


        [SerializeField] float baseDamage = 11f;
        [SerializeField] Weapon CurrentWeaponConfig = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;

        [Range(.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float CriticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem criticalHitParticle =null;




        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";


        Enemy enemy = null;
        Animator animator = null;
        float currentHealthPoints = 0;
        CameraRaycaster cameraRaycaster = null;
        float lastHitTime = 0f;
        GameObject weaponObject;
        SpecialAbilities abilities;



        void Start()
        {
            RegisterForMouseClick();
            PutWeaponInHand(CurrentWeaponConfig);
            SetAttackAnimation();
            abilities = GetComponent<SpecialAbilities>();
        }

        public void PutWeaponInHand(Weapon weaponToUse)
        {
            CurrentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject mainHand = RequestMainHand();
            Destroy(weaponObject); //empty out player hands
            weaponObject = Instantiate(weaponPrefab, mainHand.transform);
            weaponObject.transform.localPosition = CurrentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = CurrentWeaponConfig.gripTransform.localRotation;
        }


        private void Update()
        {
            var healthPercentage = GetComponent<HealthSystem>().healthAsPercentage;
            if (healthPercentage > Mathf.Epsilon) // 
            {
                ScanForAbilityKeyDown();
            }
        }

        private void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++) // starting at 1, loop through the total abilities created
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    abilities.AttempSpecialAbility(keyIndex);
                }
            }
        }

        private void SetAttackAnimation() 
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = CurrentWeaponConfig.GetAttackAnimClip();
        }

        private void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy; // adding OnMouseOverEnemy to event list for observing onMouseOverEnemy

        }

        void OnMouseOverEnemy(Enemy enemyToSet) // observing onMouseOverEnemy, when it happenes it passes (Enemy enemy) to this method
        {
            this.enemy = enemyToSet;
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject)) // enemy.gameobject selects the parent gameObject for the enemy script
            {
                AttackTarget();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                abilities.AttempSpecialAbility(0);
            }
        }

        private void AttackTarget()
        {
            if (Time.time - lastHitTime > CurrentWeaponConfig.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGGER);
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage()
        {

            bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance; // calculate crits
            float damageBeforeCritical = baseDamage + CurrentWeaponConfig.GetAdditionalDamage();
            if (isCriticalHit)
            {
                criticalHitParticle.Play();
                return damageBeforeCritical * CriticalHitMultiplier;
            }
            else
            {
                return damageBeforeCritical;
            }
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= CurrentWeaponConfig.GetMaxAttackRange();
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
// TODO consider re-wiring, don't want Player script to be affected by CameraUI, core,and weapons
using RPG.CameraUI; 
using RPG.Core; 
using RPG.Weapons;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {

        [SerializeField] float maxHealthPoints = 100;
        [SerializeField] float baseDamage = 11f;
        [SerializeField] Weapon weaponInUse = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;


        // Temporarily serialized for dugging
        [SerializeField] SpecialAbility[] abilities;

        const string DEATH_TRIGGER = "Death";
        const string ATTACK_TRIGGER = "Attack";

        AudioSource audioSource;
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
            audioSource = GetComponent<AudioSource>();
        }

        public void AdjustHealth(float changePoints)
        {
            bool playerDies = (currentHealthPoints - changePoints <= 0); // if this is true, player died.. Must ask before reducing health
            ReduceHealth(changePoints);
            if (playerDies)
            {
                ReduceHealth(changePoints);
                StartCoroutine(KillPlayer());
            }
        }

        IEnumerator KillPlayer()
        {
            // trigger deather animation
            animator.SetTrigger(DEATH_TRIGGER);
            //play death sound
            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioSource.Play();
            // wait and load the scene
            yield return new WaitForSecondsRealtime(audioSource.clip.length); 
            SceneManager.LoadScene(1); // reload scene (or go to death screen) - (Use SceneManager) 
        }

        private void ReduceHealth(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            audioSource.clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            audioSource.Play();

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
            var energyCost = abilities[abilityIndex].GetEnergyCost();  // to make it read from scripttible object

            if (energyComponent.IsEnergyAvailable(energyCost))
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
                animator.SetTrigger(ATTACK_TRIGGER);
                enemy.AdjustHealth(baseDamage);
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
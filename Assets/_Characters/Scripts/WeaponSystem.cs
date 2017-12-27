using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters

{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] float baseDamage = 11f;
        [SerializeField] WeaponConfig CurrentWeaponConfig = null;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        GameObject target;
        GameObject weaponObject;
        Animator animator = null;
        Character character;
        float lastHitTime;

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>(); // on the same GameObject that this method is on (Weaponsystem), find the character script.   find the things that defines this character

            PutWeaponInHand(CurrentWeaponConfig);
            SetAttackAnimation();
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void PutWeaponInHand(WeaponConfig weaponToUse)
        {
            CurrentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject mainHand = RequestMainHand();
            Destroy(weaponObject); //empty out player hands
            weaponObject = Instantiate(weaponPrefab, mainHand.transform);
            weaponObject.transform.localPosition = CurrentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = CurrentWeaponConfig.gripTransform.localRotation;
        }

        private void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            var animatorOverrideController = character.GetOverrideController();
            animator.runtimeAnimatorController = animatorOverrideController; // ask character what overrideController it should be using at runtime, which is the one we gave in Characters
            animatorOverrideController[DEFAULT_ATTACK] = CurrentWeaponConfig.GetAttackAnimClip();
        }

        private GameObject RequestMainHand()  // find main hand 
        {
            var mainHands = GetComponentsInChildren<MainHand>();
            int numberOfMainHands = mainHands.Length;

            Assert.IsFalse(numberOfMainHands <= 0, "No MainHand found on player, please add one"); // handle 0 hand
            Assert.IsFalse(numberOfMainHands > 1, "Multiple MainHand Scripts on Player, please remove the extra ones"); //handle 1 hand

            return mainHands[0].gameObject;

        }

        public void AttackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
            print("Attacking" + targetToAttack);
            // TODO use a repeat attack co-routine
        }

        public WeaponConfig GetCurrentWeapon() // Getter to get the CurrentWeaponConfig and make it public
        {
            return CurrentWeaponConfig;
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
            // stuff about "Critical hit" removed on RPG 159 Slowly Extracting WeaponSystem

            return baseDamage + CurrentWeaponConfig.GetAdditionalDamage();
        }
    }
}


using UnityEngine;
using RPG.CameraUI;  // for mouse events
using UnityEngine.Assertions;



// TODO remove weapon system
namespace RPG.Characters
{
    public class PlayerMovement : MonoBehaviour
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
        Character character;
        Animator animator = null;
        float currentHealthPoints = 0;
        CameraRaycaster cameraRaycaster = null;
        float lastHitTime = 0f;
        GameObject weaponObject;
        SpecialAbilities abilities;



        void Start()
        {
            character = GetComponent<Character>();

            RegisterForMouseEvent();
            PutWeaponInHand(CurrentWeaponConfig);  //todo move to WaponSystem
            SetAttackAnimation(); //todo move to WaponSystem
            abilities = GetComponent<SpecialAbilities>();
        }

        private void RegisterForMouseEvent()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy; // adding OnMouseOverEnemy to event list for observing onMouseOverEnemy
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;

        }



        private void Update()
        {
                ScanForAbilityKeyDown();
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

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                character.SetDestination(destination);
            }
        }

        private void SetAttackAnimation()      // TODO move to WeaponSystem
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = CurrentWeaponConfig.GetAttackAnimClip();
        }



        // TODO move to WeaponSystem
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


        // TODO make it better
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


        //TODO use co-routines for move and attack
        private void AttackTarget()
        {
            if (Time.time - lastHitTime > CurrentWeaponConfig.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGGER);
                lastHitTime = Time.time;
            }
        }

        // TODO move to WeaponSystem
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

        bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= CurrentWeaponConfig.GetMaxAttackRange();
        }


        private GameObject RequestMainHand()  // find main hand      // TODO move to WeaponSystem
        {
            var mainHands = GetComponentsInChildren<MainHand>();
            int numberOfMainHands = mainHands.Length;

            Assert.IsFalse(numberOfMainHands <= 0, "No MainHand found on player, please add one"); // handle 0 hand
            Assert.IsFalse(numberOfMainHands > 1, "Multiple MainHand Scripts on Player, please remove the extra ones"); //handle 1 hand

            return mainHands[0].gameObject;

        }
    }
}
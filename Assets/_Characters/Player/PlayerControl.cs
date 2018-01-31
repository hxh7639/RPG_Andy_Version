using UnityEngine;
using RPG.CameraUI;  // for mouse events
using System.Collections;

namespace RPG.Characters
{
    public class PlayerControl : MonoBehaviour
    {
        /* Critical hit removed
        [Range(.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float CriticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem criticalHitParticle =null;
        */
        Character character;
        float currentHealthPoints = 0;
        SpecialAbilities abilities;
        WeaponSystem weaponSystem;
        
        void Start()
        {
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();
            RegisterForMouseEvent();


        }

        private void RegisterForMouseEvent()
        {
            var cameraRaycaster = FindObjectOfType<CameraRaycaster>();
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
                weaponSystem.StopAttacking();
                character.SetDestination(destination);
            }
        }

        bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }

        void OnMouseOverEnemy(EnemyAI enemy) // observing onMouseOverEnemy, when it happenes it passes (Enemy enemy) to this method
        {
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject)) // enemy.gameobject selects the parent gameObject for the enemy script
            {
                weaponSystem.AttackTarget(enemy.gameObject); // pass in the enemy gameobject and it gets passed onto the attacktarget method
            }
            else if (Input.GetMouseButton(0) && !IsTargetInRange(enemy.gameObject))  // not in range
            {
                StartCoroutine(MoveAndAttack(enemy));
            }
            else if (Input.GetMouseButtonDown(1) && IsTargetInRange(enemy.gameObject))
            {
                abilities.AttempSpecialAbility(0, enemy.gameObject);
            }
            else if (Input.GetMouseButtonDown(1) && !IsTargetInRange(enemy.gameObject))
            {
                StartCoroutine(MoveAndPowerAttack(enemy));
            }
        }

        Vector3 UpdateTargetPosition()
        {
            
        }
        
        IEnumerator MoveToTarget(GameObject target)
        {
            character.SetDestination(target.transform.position);  // andy, target's position should be updated in real time.
            while (!IsTargetInRange(target))
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

        IEnumerator MoveAndAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            weaponSystem.AttackTarget(enemy.gameObject);
        }

        IEnumerator MoveAndPowerAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            abilities.AttempSpecialAbility(0, enemy.gameObject);
        }
    }
}
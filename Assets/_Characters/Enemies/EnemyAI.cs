using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Characters
{
    [RequireComponent(typeof(Character))]
    [RequireComponent (typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {        
        [SerializeField] float chaseRadius = 15f;

        PlayerMovement player = null;
        Character character;
        float currentWeaponRange;
        float distanceToPlayer;

        enum State { idle, patrolling, attacking, chasing}
        State state = State.idle;
        
        void Start()
        {
            character = GetComponent<Character>();
            player = FindObjectOfType<PlayerMovement>();
        }
        
        void Update()
        {
            // to walk to player (set player as target)
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
            if (distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                StopAllCoroutines();
                state = State.patrolling;
            }
            if (distanceToPlayer <= chaseRadius && state!= State.chasing)
            {
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }
            if (distanceToPlayer <= currentWeaponRange && state != State.attacking)
            {
                StopAllCoroutines();
                state = State.attacking;
            }

        }

        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while (distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }

        }


        //changed the way projectile works in RPG 161 simplifying enemy to enemyAI

        private void OnDrawGizmos()
        {
            // Draw attack sphere
            Gizmos.color = new Color(255f, 0f, 0, .3f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            // Draw Chase sphere
            Gizmos.color = new Color(0f, 0f, 255, .3f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
            // try out different gizmos
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Characters
{
    [RequireComponent (typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {        
        [SerializeField] float chaseRadius = 15f;

        bool isAttacking = false; // TODO more rich state
        PlayerMovement player = null;
        float currentWeaponRange;
        
        void Start()
        {
            player = FindObjectOfType<PlayerMovement>();
        }
        
        void Update()
        {
            // to walk to player (set player as target)
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
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
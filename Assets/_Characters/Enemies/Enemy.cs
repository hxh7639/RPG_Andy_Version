﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core; // TODO consider re-wire

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour
    {


        [SerializeField] float chaseRadius = 15f;

        [SerializeField] float attackRadius = 5f;
        [SerializeField] float damagePerShot = 4f;
        [SerializeField] float firingPeriodInS = 0.5f;
        [SerializeField] float firingPeriodVariation = 0.1f;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);
        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;


        bool isAttacking = false;
        Player player = null;
        


        void Start()
        {
            player = FindObjectOfType<Player>();
        }

        public void TakDamage(float amount)
        {
            // todo remove
        }

        void Update()
        {
            // to walk to player (set player as target)
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                float randomisedDelay = Random.Range(firingPeriodInS - firingPeriodVariation, firingPeriodInS + firingPeriodVariation);
                InvokeRepeating("FireProjectile", 0f, randomisedDelay); // TODO slow this down
            }
            if (distanceToPlayer > attackRadius)
            {
                isAttacking = false;
                CancelInvoke();
            }

            if (distanceToPlayer <= chaseRadius)
            {
                // aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                // aiCharacterControl.SetTarget(transform);
            }
        }

        // TODO separate out Character firing logic into another class
        void FireProjectile()
        {
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot); // set damage
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;

        }

        private void OnDrawGizmos()
        {
            // Draw attack sphere
            Gizmos.color = new Color(255f, 0f, 0, .3f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            // Draw Chase sphere
            Gizmos.color = new Color(0f, 0f, 255, .3f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
            // try out different gizmos
        }

    }
}
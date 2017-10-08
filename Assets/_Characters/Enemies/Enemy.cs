using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO consider re-wiring
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour, IDamageable
    {

        [SerializeField] float maxHealthPoints = 100f;
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
        float currentHealthPoints;
        AICharacterControl aiCharacterControl = null;

        public void TakDamage(float Damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - Damage, 0f, maxHealthPoints); //TODO switch to coroutines
            if (currentHealthPoints <= 0f)
            {
                Destroy(gameObject);
            }
        }

        public float healthAsPercentage
        {
            get
            {
                return currentHealthPoints / maxHealthPoints;
            }
        }

        void Start()
        {
            player = FindObjectOfType<Player>();
            aiCharacterControl = GetComponent<AICharacterControl>();
            currentHealthPoints = maxHealthPoints;
        }

        void Update()
        {
            if (player.healthAsPercentage <= Mathf.Epsilon)
            {
                StopAllCoroutines();
                Destroy(this);
            }
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
                aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                aiCharacterControl.SetTarget(transform);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;



public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float chaseRadius = 15f;

    [SerializeField] float attackRadius = 5f;
    [SerializeField] float damagePerShot = 4f;
    [SerializeField] float secondsBetweenShots = 0.5f;

    [SerializeField] GameObject projectileToUse;
    [SerializeField] GameObject projectileSocket;

    bool isAttacking = false;
    GameObject player = null;
    float currentHealthPoints = 100f;
    AICharacterControl aiCharacterControl = null;

    public void TakeDamage(float Damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - Damage, 0f, maxHealthPoints); //TODO switch to coroutines
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
        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();
    }

    void Update()
    {
        // to walk to player (set player as target)
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= attackRadius && !isAttacking)
        {
            isAttacking = true;
            InvokeRepeating ("SpawnProjectile", 0f, secondsBetweenShots); // TOD slow this down
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

    void SpawnProjectile()
    {
        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.SetDamage(damagePerShot); // set damage

        Vector3 unitVectorToPlayer = (player.transform.position - projectileSocket.transform.position).normalized;
        float projectileSpeed = projectileComponent.projectileSpeed;
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

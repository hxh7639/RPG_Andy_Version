using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;



public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float attackRadius = 100f;
    GameObject player = null;

    float currentHealthPoints = 100f;
    AICharacterControl aiCharacterControl = null;

    public void TakeDamage(float Damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - Damage, 0f, maxHealthPoints);
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
        if (distanceToPlayer <= attackRadius)
        {
            aiCharacterControl.SetTarget (player.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }
    }



}

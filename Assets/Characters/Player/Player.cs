using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {

    [SerializeField] float enemyLayer = 9;
    [SerializeField] float maxHealthPoints = 100;
    [SerializeField] float damagePerHit = 10f;
    [SerializeField] float minTimeBetweenHits = .5f;
	[SerializeField] float maxAttackRange = 2f;
    [SerializeField] Weapon weaponInUse;



    GameObject currentTarget;
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
        currentHealthPoints = maxHealthPoints;
        SpawnWeaponInHand();
    }

    private void RegisterForMouseClick()
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
    }

    void OnMouseClick(RaycastHit raycastHit, int layerHit)
    {
        if (layerHit == enemyLayer)
        {
            var enemy = raycastHit.collider.gameObject;

			// check if enemy is in range, if not, leave function
			if ((enemy.transform.position - transform.position).magnitude > maxAttackRange) 
			{
				return;
			}
			// if it is in range, set enemy as target and damage it
			currentTarget = enemy;
            var enemyComponent = enemy.GetComponent<Enemy>();
            if (Time.time - lastHitTime > minTimeBetweenHits)
            {
                enemyComponent.TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }
        }
    }

    public void TakeDamage(float Damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - Damage, 0f, maxHealthPoints);
    }

    void SpawnWeaponInHand()
    {
        var weaponPrefab = weaponInUse.GetWeaponPrefab();
        var weapon = Instantiate(weaponPrefab);
        // TODO move to correct place and child to hand


    }

}

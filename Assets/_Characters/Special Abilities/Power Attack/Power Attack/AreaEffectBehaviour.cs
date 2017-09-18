using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Core;

public class AreaEffectBehaviour : MonoBehaviour, ISpecialAbility
{
    AreaEffectConfig config;
    public void SetConfig(AreaEffectConfig configToSet) // setter
    {
        this.config = configToSet;
    }

    // Use this for initialization
    void Start()
    {
        print("Area Effect behaviour attached to " + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Use(AbilityUseParams useParams)
    {
        print("Area Effect used by: " + gameObject.name);
        // Static sphere cast for targets
        RaycastHit[] hits = Physics.SphereCastAll(
            transform.position, 
            config.GetRadius(), 
            Vector3.up, // direction does not matter so just pick a direction 
            config.GetRadius()
            );

        // for each hit
        foreach (RaycastHit hit in hits)
        {
            var damageable = hit.collider.gameObject.GetComponent<IDamageable>(); // see if the hit object has an IDamageable component
            // if damagable 
            if (damageable != null)
            {
                // deal damage to target + player base damage
                float damageToDeal = useParams.baseDamage + config.GetDamageToEachTarget(); // AOE damage calulation
                damageable.TakeDamage(damageToDeal);  // different ways to take damage, different from PowerAttackBehaviour script
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Core;
using System;

public class AreaEffectBehaviour : AbilityBehaviour
{
    public override void Use(GameObject target)
    {
        PlayAbilitySound();
        DealRadialDamage();
        PlayParticleEffect();
    }



    private void DealRadialDamage()
    {
         // Static sphere cast for targets
        RaycastHit[] hits = Physics.SphereCastAll(
            transform.position,
            (config as AreaEffectConfig).GetRadius(),
            Vector3.up, // direction does not matter so just pick a direction 
            (config as AreaEffectConfig).GetRadius()
            );

        // for each hit
        foreach (RaycastHit hit in hits)
        {
            var damageable = hit.collider.gameObject.GetComponent<HealthSystem>(); // see if the hit object has an HealthSystem component
            bool hitPlayer = hit.collider.gameObject.GetComponent<PlayerMovement>();
            if (damageable != null && !hitPlayer)
            {
                // deal damage to target + player base damage
                float damageToDeal = (config as AreaEffectConfig).GetDamageToEachTarget(); // AOE damage calulation
                damageable.TakDamage(damageToDeal);  // different ways to take damage, different from PowerAttackBehaviour script
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            PlayAbilitySound();
            DealDamage(target);
            PlayParticleEffect();
            transform.LookAt(target.transform); // added by andy
            PlayAbilityAnimation();
        }


        private void DealDamage(GameObject target)
        {            
            float damageToDeal = (config as PowerAttackConfig).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakDamage(damageToDeal); // if there is a healthsystem, it is damageable.  lecture 153. Eliminate A Struct And Interface
        }
    }
}

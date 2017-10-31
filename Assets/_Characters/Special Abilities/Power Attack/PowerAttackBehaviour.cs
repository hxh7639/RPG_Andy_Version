using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            DealDamage(target);
            PlayParticleEffect();
        }


        private void DealDamage(GameObject target)
        {
            PlayAbilitySound();
            float damageToDeal = (config as PowerAttackConfig).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakDamage(damageToDeal); // if there is a healthsystem, it is damageable.  lecture 153. Eliminate A Struct And Interface
        }
    }
}

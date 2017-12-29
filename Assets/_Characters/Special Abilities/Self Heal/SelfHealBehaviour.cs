using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        PlayerControl player = null;
        HealthSystem healthsystem;

        private void Start()
        {
            player = GetComponent<PlayerControl>();
            healthsystem = GetComponent<HealthSystem>(); 

        }

        public override void Use(GameObject target)
        {
            PlayAbilitySound();
            var playerHealth = player.GetComponent<HealthSystem>();
                playerHealth.Heal((config as SelfHealConfig).GetExtraHealth());
                PlayParticleEffect();
                PlayAbilityAnimation();

        }


    }
}

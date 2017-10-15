using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
        AudioSource audioSource = null;


        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }


        public override void Use(AbilityUseParams useParams)
        {
            DealDamage(useParams);
            PlayParticleEffect();
            audioSource.clip = config.getAudioClip();
            audioSource.Play();
        }


        private void DealDamage(AbilityUseParams useParams)
        {
            float damageToDeal = useParams.baseDamage + (config as PowerAttackConfig).GetExtraDamage();
            useParams.target.TakDamage(damageToDeal);
        }
    }
}

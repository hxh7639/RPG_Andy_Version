﻿using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour {

        protected AbilityConfig config; // "protected" allows it's chirdrens to see it/ call it

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK_STATE = "DEFAULT ATTACK";
        const float PARTICLE_CLEAN_UP_DELAY = 1f;

        public abstract void Use(GameObject target = null);

        public void SetConfig(AbilityConfig configToSet) // setter
        {
            config = configToSet;
        }

        protected void PlayParticleEffect()
        {
            var particlePrefab = config.GetParticlePrefab();
            var particleObject = Instantiate(
                particlePrefab, 
                transform.position, 
                particlePrefab.transform.rotation); // create the particle prefab at its current location (area effect, which is the player's location)
            particleObject.transform.parent = transform;// set to world space in prefab if required  // set particle object's transform to this object's (player) transform... spawning as a child
            particleObject.GetComponent<ParticleSystem>().Play(); // find the particle system on the prefab and play it 
            StartCoroutine(DestoryParticleWhenFinished(particleObject)); // object is the one we instantiated in game, particlePrefab is the actual prefab
            
        }

        IEnumerator DestoryParticleWhenFinished(GameObject particlePrefab)
        {
            while (particlePrefab.GetComponent<ParticleSystem>().isPlaying) // white particle effect is playing, wait for x seconds 
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            }
            Destroy(particlePrefab);
            yield return new WaitForEndOfFrame(); // just returning something because corutine is expecting us to return something 
        }

        // RPG 167 Special Ability Animations - confusing and didn't get it at the time.
        protected void PlayAbilityAnimation() 
        {
            var animatorOverrideController = GetComponent<Character>().GetOverrideController();
            var animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK_STATE] = config.GetAbilityAnimation();
            animator.SetTrigger(ATTACK_TRIGGER);

        }

        protected void PlayAbilitySound()
        {
            var abilitySound = config.GetRandomAbilitySound();
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(abilitySound); // using PlayOneShot, sound would play on top of each other w/o being cut out
        }

    }
}

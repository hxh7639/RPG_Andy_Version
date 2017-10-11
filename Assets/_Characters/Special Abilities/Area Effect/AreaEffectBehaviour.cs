using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Core;
using System;

public class AreaEffectBehaviour : MonoBehaviour, ISpecialAbility
{
    AreaEffectConfig config;
    AudioSource audioSource = null;

    public void SetConfig(AreaEffectConfig configToSet) // setter
    {
        this.config = configToSet;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    public void Use(AbilityUseParams useParams)
    {
        DealRadialDamage(useParams);
        PlayParticleEffect();
        audioSource.clip = config.getAudioClip();
        audioSource.Play();
    }

    private void PlayParticleEffect()
    {
        var particlePrefab = config.GetParticlePrefab();
        var prefab = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation); // create the particle prefab at its current location (area effect, which is the player's location)
        // TODO decide if particle system attaches to player
        ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>(); // myParticleSystem set as the particle system on my prefab 
        myParticleSystem.Play();
        Destroy(prefab, myParticleSystem.main.duration);  // could use coroutines but its much more complicated, not necessary in this case.   destroy so it is not in the game anymore.
    }

    private void DealRadialDamage(AbilityUseParams useParams)
    {
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
            bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
            if (damageable != null && !hitPlayer)
            {
                // deal damage to target + player base damage
                float damageToDeal = useParams.baseDamage + config.GetDamageToEachTarget(); // AOE damage calulation
                damageable.TakDamage(damageToDeal);  // different ways to take damage, different from PowerAttackBehaviour script
            }
        }
    }
}

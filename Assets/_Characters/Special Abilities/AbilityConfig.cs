using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public struct AbilityUseParams
    {
        public IDamageable target;
        public float baseDamage;

        public AbilityUseParams(IDamageable target, float baseDamage)  // this is a contstructor.. google it .. Lecture 112
        {
            this.target = target;
            this.baseDamage = baseDamage;
        }
    }

    public abstract class AbilityConfig : ScriptableObject  // abstract class, see lecture 109 Storing special ability
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab = null;
        [SerializeField] AudioClip[] audioClips = null;

        protected AbilityBehaviour behaviour;

        abstract public void AttachComponentTo(GameObject gameObejctToAttachTo);

        public void Use(AbilityUseParams useParams)
        {
            behaviour.Use(useParams);
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }

        public GameObject GetParticlePrefab()
        {
            return particlePrefab;

        }

        public AudioClip GetRandomAbilitySound()
        {
            return audioClips[Random.Range(0, audioClips.Length)];  // a random btw 0 and however many clips we have
        }

    }

}
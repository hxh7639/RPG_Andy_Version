using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility
    {
        PowerAttackConfig config;
        public void SetConfig(PowerAttackConfig configToSet)
        {
            this.config = configToSet;
        }

        // Use this for initialization
        void Start()
        {
            print("Power Attack behaviour attached to " + gameObject.name);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Use(AbilityUseParams useParams)
        {
            print("Power attack used by: " + gameObject.name);
            DealDamage(useParams);
            PlayParticleEffect();
        }



        private void PlayParticleEffect()
        {
            Debug.Log("AOE Particle triggered");
            var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity); // create the particle prefab at its current location (area effect, which is the player's location)
                                                                                                           // TODO decide if particle system attaches to player
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>(); // myParticleSystem set as the particle system on my prefab 
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);  // could use coroutines but its much more complicated, not necessary in this case.   destroy so it is not in the game anymore.
        }

        private void DealDamage(AbilityUseParams useParams)
        {
            float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.TakeDamage(damageToDeal);
        }
    }
}

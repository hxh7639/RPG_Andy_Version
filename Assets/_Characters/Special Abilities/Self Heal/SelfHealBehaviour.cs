using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        SelfHealConfig config = null;
        Player player = null;
        AudioSource audioSource = null;


        private void Start()
        {
            player = GetComponent<Player>();
            audioSource = GetComponent<AudioSource>();
        }

        public void SetConfig (SelfHealConfig configToSet)
        {
            this.config = configToSet;

        }

        public override void Use(AbilityUseParams useParams)
        {
            player.Heal(config.GetExtraHealth());
            audioSource.clip = config.getAudioClip();  //TODO find way of moving audio to parent class
            audioSource.Play();
            PlayParticleEffect();
        }

        private void PlayParticleEffect()
        {
            var particlePrefab = config.GetParticlePrefab();
            var prefab = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation); // create the particle prefab at its current location (area effect, which is the player's location)
            prefab.transform.parent = transform; //decide if particle system attaches to player, with this line it follows the player
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>(); // myParticleSystem set as the particle system on my prefab 
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);  // could use coroutines but its much more complicated, not necessary in this case.   destroy so it is not in the game anymore.
        }

    }
}

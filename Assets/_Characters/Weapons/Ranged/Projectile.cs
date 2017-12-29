using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// TODO consider re-wiring
using RPG.Core;

namespace RPG.Characters
{
    public class Projectile : MonoBehaviour
    {


        [SerializeField] float projectileSpeed;
        [SerializeField] GameObject shooter; // So we can inspect when paused

        const float DESTORY_DELAY = 0.01f; // because its the "magic number", so replace it with a const. (why const and not just float??)
        float damageCaused;

        public void SetShooter(GameObject shooter) // because you can't access it through serializefiled - have to access it through a setter.
        {
            this.shooter = shooter;
        }

        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }

        public float GetDefaultLaunchSpeed()
        {
            return projectileSpeed;
        }

        void OnCollisionEnter(Collision collision)
        {
            var layerCollidedWith = collision.gameObject.layer;
            if (shooter && layerCollidedWith != shooter.layer) //TODO Bug to be fixed later in course (my thought is to move coolidedwith shooter layer to the damageable area and exclude it with the if statement.
            {
                // DamageDamageable(collision);
            }
        }

    }
}
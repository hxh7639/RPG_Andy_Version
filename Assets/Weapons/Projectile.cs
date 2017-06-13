using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {


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
        var layCollidedWith = collision.gameObject.layer;
        if (layCollidedWith != shooter.layer)
        {
            DamageDamageable(collision);
        }
    }

    private void DamageDamageable(Collision collision)
    {
        Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
        if (damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
        }
        Destroy(gameObject, DESTORY_DELAY);
    }
}

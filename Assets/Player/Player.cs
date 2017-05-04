using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 100;

    float currentHealthPoints = 100f;

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / maxHealthPoints;
        }          
    }

    public void TakeDamage(float Damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - Damage, 0f, maxHealthPoints);
    }

}

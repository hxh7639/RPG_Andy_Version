using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

    public abstract class SpecialAbilityConfig : ScriptableObject  // abstract class, see lecture 109 Storing special ability
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;

        abstract public ISpecialAbility AddComponent(GameObject gameObejctToAttachTo);



    }

}
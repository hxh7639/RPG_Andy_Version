﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Power Attack"))]
    public class PowerAttackConfig : SpecialAbility
    {
        [Header("Power Attack Specifics")]
        float extraDamage = 10f;

        public override void AttachComponentTo(GameObject gameObejctToAttachTo)
        {
            var behaviourComponent = gameObejctToAttachTo.AddComponent<PowerAttackBehaviour>();
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }
        public float GetExtraDamage()
        {
            return extraDamage;
        }
    }
}

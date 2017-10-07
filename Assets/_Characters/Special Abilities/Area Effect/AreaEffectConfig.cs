using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Area Effect"))]
    public class AreaEffectConfig : SpecialAbility
    {
        [Header("Area Effect Specifics")]
        [SerializeField]float Radius = 5f;
        [SerializeField] float damageToEachTarget = 15f;

        public override void AttachComponentTo(GameObject gameObejctToAttachTo)
        {
            var behaviourComponent = gameObejctToAttachTo.AddComponent<AreaEffectBehaviour>();
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }
        public float GetRadius()
        {
            return Radius;
        }
        public float GetDamageToEachTarget()
        {
            return damageToEachTarget;
        }


    }
}
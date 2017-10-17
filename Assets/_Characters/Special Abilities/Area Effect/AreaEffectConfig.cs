using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Area Effect"))]
    public class AreaEffectConfig : AbilityConfig
    {
        [Header("Area Effect Specifics")]
        [SerializeField]float Radius = 5f;
        [SerializeField] float damageToEachTarget = 15f;

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<AreaEffectBehaviour>();
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
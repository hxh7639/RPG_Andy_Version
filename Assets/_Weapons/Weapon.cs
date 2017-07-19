using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Weapons
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class Weapon : ScriptableObject
    {

        public Transform gripTransform;

        [SerializeField] GameObject WeaponPrefab;
        [SerializeField] AnimationClip attackAnimation;

        public GameObject GetWeaponPrefab()
        {
            return WeaponPrefab;
        }

        public AnimationClip GetAttackAnimClip()
        {
            RemoveAnimationEvent();
            return attackAnimation;
        }

        // So that asset packs cannot cause crashes
        private void RemoveAnimationEvent()
        {
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}

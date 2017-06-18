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
    }
}

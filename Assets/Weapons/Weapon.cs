using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = ("RPG/Weapon"))]
public class Weapon : ScriptableObject {

    [SerializeField] GameObject WeaponPrefab;
    [SerializeField] AnimationClip attackAnimation;

    public GameObject GetWeaponPrefab() {
        return WeaponPrefab;
    }
}

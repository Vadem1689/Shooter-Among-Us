using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "GunType", menuName = "ScriptableObjects/GunType", order = 1)]
public class GunTypeSCRO : ScriptableObject
{
    public string gunName;
    public GameObject gunPrototype;
    public float fireRate;
    public float damage;
    public int ammoCount;
    public float reloadDelay;
}

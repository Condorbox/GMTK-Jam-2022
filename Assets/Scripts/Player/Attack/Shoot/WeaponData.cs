using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "Weapon")]
public class WeaponData : ScriptableObject
{
    public float damage;
    public int ammoComsum;
    [Range(0f, 1f)] public float minDispersionPercentaje;
    [Range(1f, 2f)] public float maxDispersionPercentaje;
    public PoolObjectType bulletType;
}

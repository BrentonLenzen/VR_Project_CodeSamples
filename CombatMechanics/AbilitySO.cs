using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "ScriptableObjects/Ability", order = 0)]
public class AbilitySO : ScriptableObject
{
    public enum AbilityType { PRIMARY, SPECIAL, COUNTER };

    [Header("Display Info")]
    public string name;
    public Sprite icon;

    [Header("Mechanic Info")]
    public AbilityType type;
    public float coolDown;
    public ShotTypeSO shotType; // only needs to be filled in for primary abilities
}

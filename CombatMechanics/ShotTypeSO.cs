using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShotType", menuName = "ScriptableObjects/ShotType", order = 1)]

public class ShotTypeSO : ScriptableObject
{
    public enum SpreadType { POINT, FLOWER, RANDOM, WAVE };
    public enum BulletType { STRAIGHT, SPIRAL, FALLOFF, MINE };

    [Header("Shot Type Info")]
    public int numberOfBullets;
    public SpreadType spread;
    public BulletType bullet;
    public float delay;
}

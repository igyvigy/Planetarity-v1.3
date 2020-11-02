using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRocketSO", menuName = "ScriptableObjects/RocketSO", order = 1)]
public class RocketSO : ScriptableObject
{
    public new string name;
    public float thrust;
    public float mass;
    public float size;
    public float cooldown;
    public float damage;
    public float timeToLive;
    public float minForce;
    public float maxForce;
    public float forceGain;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Machine Modifier", menuName = "ScriptableObjects/MachineModifier", order = 1)]
public class SimpleMachineModifierSO : ScriptableObject
{
    [Space]
    [Header("General Statistics")]
    public float maxHealth;
    public float maxSpeed;

    [Space]
    [Header("View Settings")]
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
}

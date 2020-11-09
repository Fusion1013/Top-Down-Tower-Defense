using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Machine Modifier", menuName = "ScriptableObjects/MachineModifier", order = 1)]
public class SimpleMachineModifierSO : ScriptableObject
{
    [Header("Max Health")]
    public ModifierType maxHealthModifierType;
    public float maxHealth;
    [Header("Max Speed")]
    public ModifierType maxSpeedModifierType;
    public float maxSpeed;

    [Header("View Radius")]
    public ModifierType viewRadiusModifierType;
    public float viewRadius;
    [Header("View Angle")]
    public ModifierType viewAngleModifierType;
    public float viewAngle;

    public enum ModifierType
    {
        Additative,
        Multiplative,
        Override
    }
}

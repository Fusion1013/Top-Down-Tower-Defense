using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Machine", menuName = "ScriptableObjects/Machine", order = 1)]
public class MachineScriptableObject : ScriptableObject
{
    [Header("Machine Settings")]
    public MachineType machineType;
    public MovementType movementType;
    public ShooterType shooterType;

    [Space]
    [Header("General Statistics")]
    public float maxHealth;
    public float maxSpeed;

    [Space]
    [Header("View Settings")]
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    [Space]
    [Header("Shooter Settings")]
    public float fireCooldown;
    public LayerMask targetLayer;

    [Space]
    [Header("Reference Objects")]
    public GameObject missile;
    public GameObject particleOnDeath;

    [ContextMenu("Choose Random Values")]
    private void ChooseRandomValues()
    {
        maxHealth = Random.Range(10, 1000);
        maxSpeed = Random.Range(1f, 20f);
        fireCooldown = Random.Range(0.1f, 4);
    }

    public enum MachineType
    {
        Enemy,
        Turret,
        Building
    }

    public enum ShooterType
    {
        None,
        Single,
        Tripple
    }

    public enum MovementType
    {
        None,
        Rotation,
        BasicEnemy
    }
}

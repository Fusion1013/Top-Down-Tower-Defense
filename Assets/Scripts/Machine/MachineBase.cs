using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MachineScriptableObject;
using static SimpleMachineModifierSO;

public class MachineBase : MonoBehaviour
{
    /*
     * IMPORTANT NOTES:
     * THE MACHINE BASE SHOULD ONLY SEND CALLS TO SHOOTERCONTROLLER
     * THE MACHINE BASE SCRIPT SHOULD NEVER CALL ANY OTHER EXTERNAL SCRIPTS
     */

    // Machine Type Settings
    [Header("Machine Type Settings")]
    public MachineType machineType;
    public MovementType movementType;
    public ShooterType shooterType;

    // Machine Modifications
    public List<SimpleMachineModifierSO> machineModifiers = new List<SimpleMachineModifierSO>();
    
    private bool machineEnabled = false;
    
    // General Statistics
    private float maxHealth;
    private float currentHealth;
    private float maxSpeed;
    private float currentSpeed;

    // View Settings
    private float viewRadius;
    private float viewAngle;

    // Shooter Settings
    private ShooterController shooterController;
    private float fireCooldown;
    private float timer;
    private LayerMask targetLayer;

    // Reference Objects
    private GameObject missile;
    private GameObject particleOnDeath;
    private GameObject target;

    void Start()
    {
        // Instantiate variables
        currentHealth = maxHealth;
        currentSpeed = maxSpeed;

        // Get Reference Scripts
        if (shooterType != ShooterType.None)
        {
            shooterController = GetComponent<ShooterController>();
            shooterController.projectilePrefab = missile;
        }

        if (machineType == MachineType.Enemy)
        {
            EnableMachine();
        }
    }

    private void LoadModifiers()
    {
        SimpleMachineModifierSO defaultModifier = machineModifiers[0];
        maxHealth = CalculateModifier(maxHealth, defaultModifier.maxHealth, defaultModifier.maxHealthModifierType);
        maxSpeed = CalculateModifier(maxSpeed, defaultModifier.maxSpeed, defaultModifier.maxSpeedModifierType);
        viewRadius = CalculateModifier(viewRadius, defaultModifier.viewRadius, defaultModifier.viewRadiusModifierType);
        viewAngle = CalculateModifier(viewAngle, defaultModifier.viewAngle, defaultModifier.viewAngleModifierType);

        for (int i = 1; i < machineModifiers.Count; i++)
        {
            SimpleMachineModifierSO modifier = machineModifiers[i];

            maxHealth = CalculateModifier(maxHealth, modifier.maxHealth, modifier.maxHealthModifierType);
            maxSpeed = CalculateModifier(maxSpeed, modifier.maxSpeed, modifier.maxSpeedModifierType);
            viewRadius = CalculateModifier(viewRadius, modifier.viewRadius, modifier.viewRadiusModifierType);
            viewAngle = CalculateModifier(viewAngle, modifier.viewAngle, modifier.viewAngleModifierType);
        }
    }

    private float CalculateModifier(float pre, float value, ModifierType type)
    {
        switch (type)
        {
            case ModifierType.Additative:
                return pre + value;
            case ModifierType.Multiplative:
                return pre * value;
            case ModifierType.Override:
                return value;
            default:
                break;
        }

        return pre;
    }

    void Update()
    {
        if (machineEnabled)
        {
            // Essentials
            Move();
            Shoot();
            DeathDetection();
        }
    }

    void Move()
    {
        switch (movementType)
        {
            case MovementType.None:
                // Do Nothing
                break;
            case MovementType.Rotation:
                Rotate();
                break;
            case MovementType.BasicEnemy:
                MoveBasicEnemy();
                break;
            default:
                break;
        }
    }

    void Shoot()
    {
        if (timer >= fireCooldown && target != null)
        {
            // Decides which method to call based on the shooter type
            switch (shooterType)
            {
                case ShooterType.None:
                    // Do nothing
                    break;
                case ShooterType.Single:
                    shooterController.SingleShot();
                    break;
                case ShooterType.Tripple:
                    shooterController.TrippleShot();
                    break;
                default:
                    break;
            }

            timer = 0;
        }
        else if (target != null)
        {
            timer += Time.deltaTime;
        }
    }

    void DeathDetection()
    {
        if (machineType == MachineType.Enemy)
        {
            GameObject[] hubs = GameObject.FindGameObjectsWithTag("HUB");
            if (hubs.Length > 0)
            {
                GameObject hub = hubs[Random.Range(0, hubs.Length - 1)];

                if (Vector3.Distance(hub.transform.position, transform.position) <= 2)
                {
                    hub.GetComponent<MachineBase>().DamageMachine(currentHealth);
                    currentHealth = 0;
                }
            }
        }

        if (currentHealth <= 0)
        {
            if (particleOnDeath != null)
            {
                Instantiate(particleOnDeath, transform.position, Quaternion.identity);
            }

            Destroy(this.gameObject);
        }
    }

    void MoveBasicEnemy()
    {
        if (target == null)
        {
            GameObject[] hubs = GameObject.FindGameObjectsWithTag("HUB");

            if (hubs.Length > 0)
            {
                GameObject hub = hubs[Random.Range(0, hubs.Length - 1)];
                transform.position = Vector3.MoveTowards(transform.position, hub.transform.position, currentSpeed * Time.deltaTime);
                transform.LookAt(hub.transform);
            }
        }
        else if (Vector3.Distance(target.transform.position, transform.position) > viewRadius / 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, currentSpeed * Time.deltaTime);
            transform.LookAt(target.transform.position);
        }
    }

    void Rotate()
    {
        if (target == null)
        {
            transform.RotateAround(transform.position, Vector3.up, currentSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 targetPoint = target.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * currentSpeed / 10);
        }
    }

    /*---Getters & Setters---*/

    public float GetMaxHealth() { return maxHealth; }

    public float GetCurrentHealth() { return currentHealth; }

    public float GetCurrentSpeed() { return currentSpeed; }

    public float GetViewRadius() { return viewRadius; }

    public float GetViewAngle() { return viewAngle; }

    public LayerMask GetTargetLayerMask() { return targetLayer; }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void DamageMachine(float damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
    }

    public void EnableMachine()
    {
        machineEnabled = true;
    }

    public void ClearTarget()
    {
        target = null;
    }
}

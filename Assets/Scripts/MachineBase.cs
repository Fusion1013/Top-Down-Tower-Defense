using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MachineScriptableObject;

public class MachineBase : MonoBehaviour
{
    /*
     * IMPORTANT NOTES:
     * THE MACHINE BASE SHOULD ONLY SEND CALLS TO SHOOTERCONTROLLER
     * THE MACHINE BASE SCRIPT SHOULD NEVER CALL ANY OTHER EXTERNAL SCRIPTS
     */

    // Scriptable Object holding all machine data
    public MachineScriptableObject machineScript;
    private bool machineEnabled = false;

    // Machine Type Settings
    private MachineType machineType;
    private MovementType movementType;
    private ShooterType shooterType;
    
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

    // Machine Modifications
    public List<SimpleMachineModifierSO> machineModifiers = new List<SimpleMachineModifierSO>();

    void Start()
    {
        // Load MachineScript
        machineType = machineScript.machineType;
        movementType = machineScript.movementType;
        shooterType = machineScript.shooterType;
        maxHealth = machineScript.maxHealth;
        maxSpeed = machineScript.maxSpeed;
        viewRadius = machineScript.viewRadius;
        viewAngle = machineScript.viewAngle;
        fireCooldown = machineScript.fireCooldown;
        targetLayer = machineScript.targetLayer;
        missile = machineScript.missile;
        particleOnDeath = machineScript.particleOnDeath;

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
        foreach (SimpleMachineModifierSO modifier in machineModifiers)
        {

        }
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

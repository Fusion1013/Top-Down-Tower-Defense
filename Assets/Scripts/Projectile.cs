using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;

    public float velocity;
    public float damage;

    private float lifetime = 5f;
    private float currentLife = 0f;

    private Quaternion targetDirection;
    private Quaternion actualDirection;
    public float smoothing;
    public float wiggleTime;

    public int obstacleMask;

    public GameObject explosion;

    private float time;

    void Start()
    {
        time = Random.Range(0, Mathf.PI * 2);
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    private void Update()
    {
        Vector3 dirWiggle = Quaternion.AngleAxis(Mathf.Sin(time * wiggleTime) * 10, Vector3.up) * direction;
        targetDirection = Quaternion.LookRotation(dirWiggle, Vector3.up);
        actualDirection = Quaternion.Lerp(transform.rotation, targetDirection, smoothing);
        transform.rotation = actualDirection;
        currentLife += Time.deltaTime;
        if (currentLife >= lifetime)
        {
            Destroy(this.gameObject);
        }
        
        transform.Translate(Vector3.forward * velocity * Time.deltaTime);

        time += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        MachineBase machineBase = other.gameObject.GetComponent<MachineBase>();
        if (machineBase != null)
        {
            machineBase.DamageMachine(damage);
        }

        Instantiate(explosion, transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }
}

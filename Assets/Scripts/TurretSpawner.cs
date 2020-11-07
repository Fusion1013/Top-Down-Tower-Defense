using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MachineScriptableObject;

public class TurretSpawner : MonoBehaviour
{
    [Header("Turret Types")]
    public GameObject singleTurret;
    public GameObject trippleTurret;

    [Space]
    [Header("Raycasting Variables")]
    public float distance = 100f;
    public int groundLayer;

    [Space]
    [Header("Misc")]
    public GameObject spawnEffect;

    private bool holdingTurret;
    private GameObject heldTurret;
    private GameObject currentPrefab;

    void Update()
    {
        if (holdingTurret)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distance))
            {
                // Clamp position
                heldTurret.transform.position = new Vector3(Mathf.Round(hit.point.x + .5f) - .5f, 0.5f, Mathf.Round(hit.point.z + .5f) - .5f);
            }

            if (Input.GetMouseButtonDown(0))
            {
                holdingTurret = false;
                heldTurret.tag = "Turret";
                MachineBase machBase = heldTurret.GetComponent<MachineBase>();
                machBase.EnableMachine();
                heldTurret.GetComponent<FieldOfView>().visualizeFOV = false;
                heldTurret.GetComponent<SphereCollider>().enabled = true;
                Instantiate(spawnEffect, heldTurret.transform.position, Quaternion.identity);
                heldTurret = null;

                if (Input.GetButton("Fire1"))
                {
                    PlaceTurret(currentPrefab);
                }
                else
                {
                    currentPrefab = null;
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                holdingTurret = false;
                currentPrefab = null;
                Destroy(heldTurret);
            }
        }
    }

    private void PlaceTurret(GameObject prefab)
    {
        holdingTurret = true;
        heldTurret = Instantiate(prefab, transform.position, Quaternion.identity);
        heldTurret.tag = "Placing";
        heldTurret.GetComponent<FieldOfView>().visualizeFOV = true;
        heldTurret.GetComponent<SphereCollider>().enabled = false;
        currentPrefab = prefab;
    }

    public void PlaceSingleTurret()
    {
        PlaceTurret(singleTurret);
    }

    public void PlaceTrippleTurret()
    {
        PlaceTurret(trippleTurret);
    }
}

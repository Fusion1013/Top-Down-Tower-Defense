using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    public GameObject projectilePrefab;

    public void SingleShot()
    {
        ShootTowards(transform.forward);
    }

    public void TrippleShot()
    {
        Vector3 vec1 = Quaternion.AngleAxis(-15, Vector3.up) * transform.forward;
        Vector3 vec2 = Quaternion.AngleAxis(15, Vector3.up) * transform.forward;

        Debug.DrawLine(transform.position, vec1, Color.red);
        Debug.DrawLine(transform.position, vec2, Color.red);

        ShootTowards(transform.forward);
        ShootTowards(vec1);
        ShootTowards(vec2);
    }

    private void ShootTowards(Vector3 dir)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position + dir, Quaternion.identity);
        BulletController controller = projectile.GetComponent<BulletController>();
        controller.direction = dir;
    }
}

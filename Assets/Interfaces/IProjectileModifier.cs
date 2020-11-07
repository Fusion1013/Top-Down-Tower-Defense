using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileModifier
{
    GameObject ModifyProjectile(GameObject projectile);
}

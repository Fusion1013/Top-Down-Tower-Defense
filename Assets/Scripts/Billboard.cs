using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform parent;

    private void LateUpdate()
    {
        Quaternion rot = Quaternion.Euler(90, 0, 0);
        
        transform.rotation = rot;
    }
}

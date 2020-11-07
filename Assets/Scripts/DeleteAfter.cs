using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfter : MonoBehaviour
{
    public float time;

    void Start()
    {
        StartCoroutine("StartTimer");
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(time);

        Destroy(this.gameObject);
    }
}

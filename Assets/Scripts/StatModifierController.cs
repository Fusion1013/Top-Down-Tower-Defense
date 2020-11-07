using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifierController : MonoBehaviour
{
    /* This class handles the modification of a machines statistics */
    /* This script should only be applied to objects that have a machine script */

    public List<StatModifierScriptableObject> modifiers = new List<StatModifierScriptableObject>();

    private MachineBase machineBase;

    void Start()
    {
        machineBase = GetComponent<MachineBase>();
        ApplyAllModifiers();
    }

    public void ApplyAllModifiers()
    {
        foreach (StatModifierScriptableObject modifier in modifiers)
        {
            // TODO: Apply modifier to machine base
        }
    }
}

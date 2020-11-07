using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    private float maxHealth;
    private float currentHealth;
    private MachineBase machineBase;

    public Slider healthSlider;
    public float updateDelay;

    private void Start()
    {
        machineBase = GetComponent<MachineBase>();
        maxHealth = machineBase.GetMaxHealth();
        currentHealth = maxHealth;

        StartCoroutine("UpdateHealth", updateDelay);
    }
    
    private IEnumerator UpdateHealth(float delay)
    {
        while (true)
        {
            maxHealth = machineBase.GetMaxHealth();
            currentHealth = machineBase.GetCurrentHealth();
            UpdateSlider();

            yield return new WaitForSeconds(delay);
        }
    }

    public void UpdateSlider()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : SingletonBehaviour<HealthManager>
{
    public int maxHealth;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        GameManager.OnReset += this.ResetHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void RemoveHealth(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            GameManager.Instance.KillRespawnPlayer();
        }
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    private void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}

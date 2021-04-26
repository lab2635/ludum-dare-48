using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damagePerHit;
    public float damageCooldown;

    private float damageElapsed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        damageElapsed += Time.fixedDeltaTime;
    }

    public int GetDamageAmount()
    {
        if (damageElapsed >= damageCooldown)
        {
            damageElapsed = 0;
            return damagePerHit;
        }
        return 0;
    }
}

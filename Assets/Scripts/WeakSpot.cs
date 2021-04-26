using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakSpot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (!enemy) return;

        int damage = enemy.GetDamageAmount();
        AudioSource source = GetComponent<AudioSource>();
        source.Play();
        HealthManager.Instance.RemoveHealth(damage);
    }
}

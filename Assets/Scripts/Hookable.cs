using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookable : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Hook hook;
    private BasicFishMovement fishMovement;

    public float breakawayChance = 0.995f;
    public Vector3 acceleration;
    public UpgradeType upgradeType;
    public bool hooked;

    public Hook Hook { get => hook; private set => hook = value; }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        hooked = false;
        fishMovement = GetComponent<BasicFishMovement>();
    }

    public bool BreakFree()
    {
        return Random.value > (1.0 - breakawayChance);
    }

    public bool AttemptHook()
    {
        return true;
    }

    public bool Attach(Hook hooker)
    {
        if (hooked) return false;

        hooked = true;
        hook = hooker;
        if (fishMovement) fishMovement.aiEnabled = false;
        gameObject.layer = LayerMask.NameToLayer("Hooked");

        // NOTE: return false to prevent hooking
        return true;
    }

    public void Detach()
    {
        hook = null;
        hooked = false;
        if (fishMovement) fishMovement.aiEnabled = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    void FixedUpdate()
    {
        if (!hooked) return;
        rigidbody.MovePosition(hook.transform.position);
        acceleration = Vector3.down * 0.5f;
    }
}

public enum UpgradeType
{
    Spotlight,
    Sonar,
    Scare,
    Boost,
    Sight,
    Goal,
    None
}

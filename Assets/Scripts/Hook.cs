using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class Hook : MonoBehaviour
{
    public AudioSource hookAudio;
    public ParticleSystem particles;
    
    private Rigidbody body;
    private PlayerInput input;
    private Pole pole;
    private float detachingElapsed;

    private enum HookState
    {
        Idle,
        Detaching
    }

    [Flags]
    private enum HookFlags
    {
        None,
        Hooked
    }

    
    private Vector3 velocity = Vector3.zero;
    private Vector3 acceleration = Vector3.zero;
    
    private HookState state = HookState.Idle;
    private HookFlags flags;

    private Vector3 offset;
    private Hookable hookable;
    private Collider[] colliders;
    private float time = 0f;
    private float yankTimer = 0f;

    public LayerMask detectionMask;
    // public float reelStrength = 2f;
    // public float reelStrengthMultiplier = 1f;
    public float speed = 1f;
    public float speedMultiplier = 1f;
    // public float yankStrength = 1f;
    // public float yankDuration = 1f;
    // public float yankCooldown = 0.25f;
    public float sway = 0.01f;
    public float swayFrequency = 1f;
    public float detectionRadius = 1f;
    public float correctionFactor = 1f;
    public Transform hookPivot;

    void Start()
    {
        colliders = new Collider[5];
        pole = GameObject.FindWithTag("Pole").GetComponent<Pole>();
        input = GetComponent<PlayerInput>();
        body = GetComponent<Rigidbody>();
        state = HookState.Idle;
        GameManager.OnReset += this.ResetHook;
    }

    void Update()
    {
        //if (input.debug) Breakaway();

        // AdjustPosition();

        //
        // switch (state)
        // {
        //     case HookState.Idle:
        //     case HookState.Falling:
        //         if (IsHooked()) AttemptBreak();
        //         if (CanReel() && input.reel)
        //         {
        //             body.AddForce(Vector3.zero, ForceMode.VelocityChange);
        //             state = HookState.Reeling;
        //             break;
        //         }
        //         if (input.yankLeft && CanYank()) YankLeft();
        //         if (input.yankRight && CanYank()) YankRight();
        //         break;
        //     case HookState.Reeling:
        //         if (input.reel) break;
        //         if (IsHooked()) AttemptBreak();
        //         state = CanFall() ? HookState.Falling : HookState.Idle;
        //         break;
        //     case HookState.Yanking:
        //         if (Detect()) HookTo();
        //         break;
        // }
        //
        // yankTimer += Time.deltaTime;

        if (Detect()) HookTo();
    }

    public bool IsHooked() => flags.HasFlag(HookFlags.Hooked);
    //
    // bool CanYank() => state == HookState.Falling && !IsHooked() && (yankTimer >= yankCooldown);
    // bool CanReel() => state == HookState.Falling || state == HookState.Reeling || IsHooked();
    // bool CanFall() => state != HookState.Yanking && !IsHooked();
    //
    // void YankLeft() => StartCoroutine(Yank(Vector3.up + Vector3.left));
    // void YankRight() => StartCoroutine(Yank(Vector3.up + Vector3.right));
    //
    // IEnumerator Yank(Vector3 direction)
    // {
    //     var prevState = state;
    //     yankTimer = 0f;
    //     state = HookState.Yanking;
    //     acceleration = direction * yankStrength;
    //     yield return new WaitForSeconds(yankDuration);
    //     state = prevState;
    // }
    //
    bool CanDetect() => !IsHooked();
    
    void HookTo()
    {
        if (state != HookState.Idle) return;
        if (hookable == null) return;
        if (!hookable.Attach(this)) return;

        particles.Play();
        flags |= HookFlags.Hooked;
        gameObject.layer = LayerMask.NameToLayer("Hooked");
        this.hookAudio.Play();
    }

    public void Breakaway()
    {
        if (hookable != null)
        {
            StartCoroutine(Detach());
        }

        particles.Stop();
        flags &= ~HookFlags.Hooked;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private IEnumerator Detach()
    {
        hookable.Detach();
        hookable = null;
        state = HookState.Detaching;
        yield return new WaitForSeconds(1f);
        state = HookState.Idle;
    }
    
    bool Detect()
    {
        if (!CanDetect()) return false;
        
        var collisions = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, colliders, detectionMask);

        if (collisions <= 0)
        {
            hookable = null;
            return false;
        }

        for (int i = 0; i < collisions; i++)
        {
            hookable = colliders[i].GetComponent<Hookable>();
            if (hookable) return true;
        }
        return false;
    }

    void AttemptBreak()
    {
        // only allow breaks when there is something hooked
        
        if (!flags.HasFlag(HookFlags.Hooked) || hookable == null) return;
        
        // unhook if the hookable wants to break free
        
        if (hookable.BreakFree())
        {
            Breakaway();
        }
    }

    void AdjustPosition()
    {
        
        // var position = transform.position;
        // var polePosition = pole.transform.position;
        // var targetPosition = polePosition + Vector3.down * pole.lineLength;
        // var distanceFromPole = Vector3.Distance(polePosition, position);
        // var direction = Vector3.Normalize(targetPosition - position);
        //
        // var destination = targetPosition + direction * pole.lineLength;
        // var interpolatedPosition = Vector3.MoveTowards(position, destination, Time.fixedDeltaTime * correctionFactor);
        // // var finalPosition = Vector3.Max(targetPosition, interpolatedPosition);
        //
        // var targetRotation = Quaternion.FromToRotation(Vector3.up, polePosition - transform.position);
        // var rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.fixedDeltaTime * 3f);
        //  
        // body.MovePosition(targetPosition);
        // body.MoveRotation(rotation);
        
        //
        //  debug = distanceFromPole;
        //
        //  if (Mathf.Abs(distanceFromPole - pole.lineLength) > 0.001f)
        //  {
        //      var parentBody = pole.GetComponentInParent<Rigidbody>();
        //      var speed = parentBody.velocity.magnitude;
        //      speed = Mathf.Clamp(speed, correctionFactor, speed);
        //      var target = polePosition + Vector3.down * pole.lineLength;
        //      var endPosition = Vector3.MoveTowards(position, target, Time.fixedDeltaTime * speed * correctionFactor);
        //      body.MovePosition(endPosition);
        //  }
        //
        //  var targetRotation = Quaternion.FromToRotation(Vector3.up, polePosition - transform.position);
        //  var rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 3f);
        //
        // body.MoveRotation(rotation);
    }
    
    void FixedUpdate()
    {
        // velocity = Vector3.zero;
        //
        // switch (state)
        // {
        //     case HookState.Idle:
        //         acceleration = Vector3.zero;
        //         if (IsHooked())
        //         {
        //             acceleration += hookable.acceleration;
        //         }
        //         break;
        //     case HookState.Reeling:
        //         var directionToPole = Vector3.Normalize(pole.transform.position - transform.position);
        //         acceleration = directionToPole * reelStrength * reelStrengthMultiplier;
        //         if (IsHooked())
        //         {
        //             acceleration += hookable.acceleration;
        //         }
        //         break;
        //     case HookState.Falling:
        //         var tau = Mathf.PI * 2f;
        //         acceleration = Vector3.down * speed * speedMultiplier;
        //         acceleration += Vector3.left * Mathf.Sin(time * tau * swayFrequency) * sway * 10.0f;
        //         break;
        //     case HookState.Yanking:
        //         break;
        //     default:
        //         acceleration = Vector3.zero;
        //         break;
        // }

        AdjustPosition();
        
        // velocity += acceleration;
        // body.MovePosition(transform.position + velocity * Time.fixedDeltaTime);

        time += Time.fixedDeltaTime;
    }

    public void ResetHook()
    {
        if (this.IsHooked())
        {
            Breakaway();
        }
        else
        {
            state = HookState.Idle;
        }
    }

    public float debug;

    public void EnterBubble()
    {
        this.speedMultiplier = 2f;
    }

    public void ExitBubble()
    {
        this.speedMultiplier = 1f;
    }

    public void HitCoral()
    {
        this.speedMultiplier = 0;
    }

    public void ExitCoral()
    {
        this.speedMultiplier = 1f;
    }

    public void EnterSeaweed()
    {
        this.speedMultiplier = 0.5f;
    }

    public void ExitSeaweed()
    {
        this.speedMultiplier = 1f;
    }
}

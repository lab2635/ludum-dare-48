using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody body;
    private PlayerInput input;
    
    public GameObject sonarPrefab;
    

    public GameObject graphics;
    public LayerMask fishDetectionMask;
    public float rotationSpeed = 3f;
    public float lowerSpeed = 1f;
    public float lowerSpeedMultiplier = 1f;
    public float raiseSpeed = 1f;
    public float raiseSpeedMultiplier = 1f;
    public float horizontalSpeed = 1f;
    public float horizontalSpeedMultiplier = 1f;
    public float angleRecoveryRate = 1f;

    public ParticleSystem boostBubbles;
    public float boostPower = 1f;
    public float boostCooldown = 1f;
    public float boostDuration = 0.5f;
    public float boostBrakeDuration = 0.2f;
    public float boostTimer;
    public float blastCooldown = 5f;
    public float blastTimer;
    public float sonarPulseTimer;
    public float sonarCooldownTime;
    public float secondPulseTime;

    public AudioSource splashAudio;
    public AudioSource engineAudio;
    public AudioSource boostAudio;
    public AudioSource sonarAudio;
    public AudioSource inkAudio;
    public AudioSource impactAudio;

    private Vector3 acceleration = Vector3.zero;

    private Quaternion facingDirection = Quaternion.Euler(0f, 90f, 0f);
    private UpgradeManager upgradeManager;
    
    private bool pulseFired;
    private bool boosting;
    private bool boostBraking;
    
    private float currentSpeed;
    private float speedMultiplier;

    void Start()
    {
        GameManager.OnReset += OnGameReset;
        
        input = GetComponent<PlayerInput>();
        body = GetComponent<Rigidbody>();
        body.centerOfMass = Vector3.zero;
        upgradeManager = UpgradeManager.Instance;

        sonarPulseTimer = 0f;
        pulseFired = false;
        speedMultiplier = 1f;
    }

    void OnDestroy()
    {
        GameManager.OnReset -= OnGameReset;
    }

    private void OnGameReset()
    {
        Reset();
    }
    
    public void Reset()
    {
        facingDirection = Quaternion.identity;
        body.velocity = Vector3.zero;
        body.drag = 0.2f;
        acceleration = Vector3.zero;
        boosting = false;
        boostBrake = false;
        pulseFired = false;
        blastTimer = 0f;
        boostTimer = 0f;
        secondPulseTime = 0f;
        sonarPulseTimer = 0f;
    }

    public int GetDepth()
    {
        var value = transform.position.y;
        var depth = Mathf.FloorToInt(-value);
        return depth <= 0 ? 0 : depth;
    }
    
    bool IsAtOrAboveSurface()
    {
        return transform.position.y >= 0f;
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlayerControllerEnabled) return;

        acceleration = Vector3.zero;

        // rotate graphics towards facing direction

        var targetRotation = transform.rotation * facingDirection;
        
        graphics.transform.rotation = Quaternion.Slerp(
            graphics.transform.rotation, targetRotation, 
            Time.deltaTime * rotationSpeed);
        
        // handle movement
        
        if (input.moveUp && !IsAtOrAboveSurface())
        {
            acceleration += Vector3.up * (raiseSpeed * raiseSpeedMultiplier);
        }
        
        if (input.moveDown)
        {
            acceleration += Vector3.down * (lowerSpeed * lowerSpeedMultiplier);
        }

        if (input.moveLeft)
        {
            acceleration += Vector3.left * (horizontalSpeed * horizontalSpeedMultiplier);
            facingDirection = Quaternion.Euler(0, 90, 0);
        }

        if (input.moveRight)
        {
            acceleration += Vector3.right * (horizontalSpeed * horizontalSpeedMultiplier);
            facingDirection = Quaternion.Euler(0, -90, 0);
        }

        if (input.releaseHook)
        {
            ReleaseHook();
        }

        if (IsAtOrAboveSurface() && !this.splashAudio.isPlaying)
        {
            this.splashAudio.Play();
        }

        // keep sub below surface
        
        if (IsAtOrAboveSurface())
        {
            acceleration = Vector3.down * 8f;
        }
        else
        {
            body.drag = 0.2f;
        }

        // sonar pulses

        if (upgradeManager.sonarUnlocked && input.sonarPulse && !pulseFired && sonarPulseTimer >= sonarCooldownTime)
        {
            StartCoroutine(PulseSonar());
        }
        
        sonarPulseTimer += Time.deltaTime;

        // headlight
        
        if (input.toggleHeadlight && UpgradeManager.Instance.spotlightUnlocked)
        {
            ToggleSpotlight();
        }

        // boost 
        
        float currentBoostCooldown = UpgradeManager.Instance.boostCooldownBase - UpgradeManager.Instance.boostCooldownModifier;

        if (!boosting && boostTimer >= currentBoostCooldown && input.boost)
        {
            boostAudio.Play();
            boostBubbles.Play();
            boosting = true;
            boostTimer = 0f;
        }
        
        if (boosting && boostTimer >= boostDuration + boostBrakeDuration)
        {
            boostBrake = true;
            boosting = false;
            boostTimer = 0f;
        }

        boostTimer += Time.deltaTime;
		
		// ink blast 		

        if (CanInkBlast())
        {
            StartCoroutine(InkBlast());
        }

        blastTimer += Time.deltaTime;

    }

    private bool CanInkBlast()
    {
        if (!input.blast) return false;
        if (blastTimer < blastCooldown) return false;
        return upgradeManager.inkBlastUnlocked && upgradeManager.HasInkBlastCharge();
    }

    IEnumerator InkBlast()
    {
        blastTimer = 0f;
        Blast();
        yield break;
    }
    
    IEnumerator PulseSonar()
    {
        this.sonarAudio.Play();

        var position = transform.position;
        var rotation = transform.rotation;
        
        sonarPulseTimer = 0f;
        pulseFired = true;
        Instantiate(sonarPrefab, position, rotation);
        yield return new WaitForSeconds(secondPulseTime);
        Instantiate(sonarPrefab, position, rotation);
        pulseFired = false;
    }

    IEnumerator Boost()
    {
        this.boostAudio.Play();
        boostTimer = 0f;
        boosting = true;
        boostBubbles.Play();
        yield return new WaitForSeconds(boostDuration);
        boosting = false;
        StartCoroutine(BoostBrake());
    }

    IEnumerator BoostBrake()
    {
        yield return new WaitForSeconds(boostBrakeDuration);
        body.velocity -= body.velocity * 0.6f;
    }

    private bool boostBrake;

    void FixedUpdate()
    {
        currentSpeed = body.velocity.magnitude;

        if (currentSpeed > 0.2 && !this.engineAudio.isPlaying)
        {
            this.engineAudio.Play();
        }
        else if (currentSpeed <= 0.2)
        {
            this.engineAudio.Pause();
        }

        if (boosting && boostTimer < boostDuration)
        {
            acceleration += acceleration.normalized * boostPower;
        }

        if (boostBrake)
        {
            body.velocity -= body.velocity * 0.6f; // * Time.fixedDeltaTime;
            boostBrake = false;
        }
        
        acceleration *= speedMultiplier;

        var targetSubRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        var angle = Quaternion.Angle(transform.rotation, targetSubRotation);
        var targetRotation = Quaternion.RotateTowards(transform.rotation, targetSubRotation,
            Time.fixedDeltaTime * angleRecoveryRate * angle);
        
        body.MoveRotation(targetRotation);
        body.AddForce(acceleration, ForceMode.Acceleration);
    }

    private void ToggleSpotlight()
    {
        Light spotlight = FindSpotlight();
        if (!spotlight) return;

        spotlight.enabled = !spotlight.enabled;
    }

    public bool isSpotlightEnabled
    {
        get
        {
            Light spotlight = FindSpotlight();
            if (!spotlight) return false;
            return spotlight.enabled;
        }
    }

    public void EnableSpotlight()
    {
        Light spotlight = FindSpotlight();
        if (!spotlight) return;

        spotlight.enabled = true;
    }

    public void DisableSpotlight()
    {
        Light spotlight = FindSpotlight();
        if (!spotlight) return;

        spotlight.enabled = false;
    }

    private Light FindSpotlight()
    {
        return GetComponentsInChildren<Light>()
            .Where(x => x.gameObject.name == "Headlight")
            .First();
    }

    private void Blast()
    {
        this.inkAudio.Play();
        UpgradeManager.Instance.ConsumeInkCharge();
        ParticleSystem inkBlast = GetComponentInChildren<ParticleSystem>();
        inkBlast.Play();

        // Find nearby fish and make them flee
        Collider[] colliders = new Collider[10];
        var collisions = Physics.OverlapSphereNonAlloc(transform.position, 5, colliders, fishDetectionMask);

        if (collisions <= 0)
        {
            return;
        }

        for (int i = 0; i < collisions; i++)
        {
            BasicFishMovement fishMovement = colliders[i].gameObject.GetComponent<BasicFishMovement>();
            if (fishMovement)
            {
                fishMovement.BeginFlee();
            }
        }
    }

    private void ReleaseHook()
    {
        GameObject hookObj = GameObject.FindGameObjectWithTag("Hook");
        Hook hook = hookObj.GetComponent<Hook>();
        hook.Breakaway();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ObstacleCollider obstacle = collision.gameObject.GetComponent<ObstacleCollider>();

        if (obstacle && obstacle.obstacleType == ObstacleCollider.ObstacleType.Rock)
        {
            if (currentSpeed > 3)
            {
                this.impactAudio.Play();
                HealthManager.Instance.RemoveHealth((int)currentSpeed);

                // Play impact sound here
            }
        }
    }

    public void EnterBubble()
    {
        this.speedMultiplier = 2f;
    }

    public void HitCoral()
    {
        this.speedMultiplier = 0.5f;
    }

    public void EnterSeaweed()
    {
        this.speedMultiplier = 0.5f;
    }

    public void ResetSpeed()
    {
        this.speedMultiplier = 1f;
    }
}

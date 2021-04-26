using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollider : MonoBehaviour
{
    public ObstacleType obstacleType;
    public float floatSpeed;
    public float lifetime;

    public AudioSource seaweedCoralAudio;

    private float elapsedTime;
    private bool isPlaying;

    public enum ObstacleType
    {
        Rock,
        Seaweed,
        Coral,
        AirBubble
    }

    // Start is called before the first frame update
    void Start()
    {
        this.isPlaying = false;
        this.elapsedTime = 0;
    }

    private void Update()
    {
        if(this.obstacleType == ObstacleType.AirBubble && elapsedTime >= lifetime)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.elapsedTime += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if(this.obstacleType == ObstacleType.AirBubble)
        {
            this.transform.position += new Vector3(0, floatSpeed * Time.fixedDeltaTime, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement movement = other.GetComponent<PlayerMovement>();
        if (!movement) return;

        switch (obstacleType)
        {
            case ObstacleCollider.ObstacleType.AirBubble:
                movement.EnterBubble();
                break;
            case ObstacleCollider.ObstacleType.Coral:
                this.PlayAudio();
                movement.HitCoral();
                break;
            case ObstacleCollider.ObstacleType.Rock:
                break;
            case ObstacleCollider.ObstacleType.Seaweed:
                this.PlayAudio();
                movement.EnterSeaweed();
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement movement = other.GetComponent<PlayerMovement>();
        if (!movement) return;

        if (this.isPlaying && obstacleType == ObstacleType.Coral || obstacleType == ObstacleType.Seaweed)
        {
            this.PauseAudio();
        }
        movement.ResetSpeed();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!this.isPlaying && obstacleType == ObstacleType.Coral || obstacleType == ObstacleType.Seaweed)
        {
            this.PlayAudio();
        }
    }

    private void PlayAudio()
    {
        if (!this.isPlaying || !this.seaweedCoralAudio.isPlaying)
        {
            if (!this.seaweedCoralAudio.isPlaying)
            {
                this.seaweedCoralAudio.Play();
            }
            else
            {
                this.seaweedCoralAudio.Pause();
            }

            this.isPlaying = true;
        }
    }

    private void PauseAudio()
    {
        this.seaweedCoralAudio.Pause();
    }
}

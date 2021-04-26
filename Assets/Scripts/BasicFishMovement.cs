using UnityEngine;
using Random = UnityEngine.Random;

public class BasicFishMovement : MonoBehaviour
{
    public int idleThinkInterval;
    public int activeThinkInterval;
    public float idleVelocity;
    public float activeVelocity;
    public int interactDistance;
    public BehaviorType behaviorType;
    public bool aiEnabled;
    public float fleeDuration = 3f;
    public GameObject graphics;
    public float rotationOffset = 0f;
    public Spawner spawnArea;
    public int maxWanderAttempts = 100;

    private Rigidbody rigidbody;
    private GameObject interactTarget;
    private float time = 0f;
    private float nextUpdate = 0f;
    private Vector3 destination;
    private FishBehavior behavior;
    private bool fleeing;
    private float fleeStart;
    private SharkAttack attack;

    // Start is called before the first frame update
    void Start()
    {
        nextUpdate = 0;
        destination = transform.position;
        behavior = FishBehavior.GetBehavior(behaviorType);
        interactTarget = GameObject.FindGameObjectWithTag("Player");
        rigidbody = GetComponent<Rigidbody>();
        attack = GetComponent<SharkAttack>();
    }
    
    private Vector3 ChooseNewDestination() => behavior.ChooseNewDestination(transform.position, interactTarget.transform.position, interactDistance, fleeing);

    private Vector3 CalculateNewDestination()
    {
        if (behavior == null) return Vector3.zero;
        if (spawnArea == null) return ChooseNewDestination();

        if (spawnArea.Overlaps(transform.position))
        {
            // Already outside the spawn zone. Just revert to standard behavior
            return ChooseNewDestination();
        }

        var wanderAttempts = maxWanderAttempts;
        var attempt = 0;
        var nextDestination = Vector3.zero;
        
        while (attempt < wanderAttempts)
        {
            // attempt to choose a destination within the spawn area
            
            nextDestination = ChooseNewDestination();
            nextDestination.z = 0f;
            
            if (!spawnArea.Overlaps(nextDestination))
            {
                return nextDestination;
            }
      
            // if the previous check failed, attempt to pick a destination
            // along the perimeter by casting a ray towards the boundary in the
            // direction the AI wants to go
            
            var direction = (nextDestination - destination).normalized;
            
            if (spawnArea.IntersectsWith(destination, direction, out var intersection))
            {
                return intersection;
            }

            attempt++;
        }
        
        return destination;
    }
    
    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if (!aiEnabled) return;

        if (time - fleeStart >= fleeDuration)
        {
            fleeing = false;
        }

        if ((time > nextUpdate || DistanceToDestination() < 0.01)
            && !IsAttacking())
        {
            rigidbody.velocity = Vector3.zero;
            destination = CalculateNewDestination();
            nextUpdate = time + NextUpdateInterval();
        }

        float velocity = idleVelocity;
        if (IsActive())
        {
            velocity = activeVelocity;
        }

        // Vector3 lookDirection = new Vector3(destination.x, transform.position.y, destination.z) - transform.position;
        // Vector3 newDir = Vector3.RotateTowards(graphics.transform.forward, lookDirection, 0.1f, 0.0f);
        // graphics.transform.rotation = Quaternion.LookRotation(newDir);

        Vector3 newVelocity;
        Vector3 direction;
        if (IsAttacking())
        {
            // Skip wandering and chasing and whatnot and run the attack AI
            direction = (interactTarget.transform.position - transform.position).normalized;
            newVelocity = attack.Attack();
        }
        else
        {
            direction = (destination - transform.position).normalized;
            newVelocity = direction * velocity;
        }
        rigidbody.velocity = newVelocity;

        Quaternion targetRotation = direction.x <= 0
            ? Quaternion.AngleAxis(0f + rotationOffset, Vector3.up)
            : Quaternion.AngleAxis(180f + rotationOffset, Vector3.up);

        graphics.transform.rotation = Quaternion.RotateTowards(graphics.transform.rotation, 
            targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    public float rotationSpeed = 180f;
    
    private bool IsAttacking()
    {
        return IsActive() && attack && !fleeing;
    }

    private float NextUpdateInterval()
    {
        if (IsActive())
        {
            return Mathf.Clamp(activeThinkInterval + Jitter(), 0, 2);
        }
        return Mathf.Clamp(idleThinkInterval + Jitter(), 0, 2);
    }

    private float DistanceToDestination()
    {
        return (transform.position - destination).magnitude;
    }

    private float DistanceToGoal()
    {
        return (transform.position - interactTarget.transform.position).magnitude;
    }

    private bool IsActive()
    {
        return DistanceToGoal() <= interactDistance;
    }

    private float Jitter()
    {
        return Random.value - 0.5f;
    }

    public void BeginFlee()
    {
        fleeing = true;
        fleeStart = time;
    }
}

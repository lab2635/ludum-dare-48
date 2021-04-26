using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAttack : MonoBehaviour
{
    public float dashDuration;
    public float dashCooldown;
    public float dashPower;
    public float followSpeed;
    public GameObject graphics;

    private Rigidbody rigidbody;
    private float dashingElapsed;
    private bool dashing = false;
    private float currentSpeed;
    private Vector3 dashDirection;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("PlayerWeakSpot");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        dashingElapsed += Time.fixedDeltaTime;
    }

    public Vector3 Attack()
    {
        // Follow and dash in on a cooldown
        if (dashingElapsed >= dashCooldown)
        {
            StartCoroutine(Dash());
        }

        currentSpeed = rigidbody.velocity.magnitude;
        Vector3 direction = (target.transform.position - transform.position).normalized;
        if (dashing)
        {
            currentSpeed = followSpeed * dashPower;
            direction = dashDirection;
        }
        else
        {
            currentSpeed = followSpeed;
        }

        return direction * currentSpeed;
    }

    private IEnumerator Dash()
    {
        dashing = true;
        dashingElapsed = 0;
        dashDirection = (target.transform.position - transform.position).normalized;
        Animator animator = graphics.GetComponent<Animator>();
        if (animator)
        {
            animator.Play("fastswim");
        }
        yield return new WaitForSeconds(dashDuration);
        if (animator)
        {
            animator.Play("swim");
        }
        dashing = false;
        StartCoroutine(DashBrake());
    }

    private IEnumerator DashBrake()
    {
        yield return new WaitForSeconds(0.2f);
        rigidbody.velocity -= rigidbody.velocity * 0.6f;
    }
}

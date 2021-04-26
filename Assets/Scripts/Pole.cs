using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Pole : MonoBehaviour
{
    private Hook hook;
    private LineRenderer lineRenderer;
    private Vector3[] positions;
    public float lineLength = 3f;
    
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.05f;
        lineRenderer.useWorldSpace = true;
        hook = GameObject.FindWithTag("Hook").GetComponent<Hook>();
        positions = new Vector3[2];
    }

    void Update()
    {
        positions[0] = transform.position;
        positions[1] = hook.hookPivot.transform.position;
        lineRenderer.SetPositions(positions);
    }
}

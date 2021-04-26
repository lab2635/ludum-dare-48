using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Pulse : MonoBehaviour
{
    public Color pulseColor = Color.green;
    public float pulseIntensity = 1f;
    public float pulseRate = 1f;

    private float time;
    private Material material;

    void Start()
    {
        var renderer = GetComponent<MeshRenderer>();
        material = Instantiate(renderer.sharedMaterial);
        renderer.sharedMaterial = material;
    }

    void Update()
    {
        var intensity = pulseIntensity * (Mathf.Sin(time * Mathf.PI * 2f * pulseRate) + 1f / 2f);
        var color = pulseColor * intensity;
        material.SetColor("_EmissionColor", color);
        time += Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class SunController : MonoBehaviour
{
    public float intensityMultiplier;
    private GameObject sub;
    private Light sunlight;

    // Start is called before the first frame update
    void Start()
    {
        this.sub = GameObject.FindGameObjectWithTag("Player");
        this.sunlight = this.gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        float intensityModifier = this.sub.transform.position.y * this.intensityMultiplier * -1;
        this.sunlight.intensity = 1 - intensityModifier;
    }
}

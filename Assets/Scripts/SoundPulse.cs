using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPulse : MonoBehaviour
{
    public float lifetime;
    public float scaleMultiplier;

    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(currentTime > lifetime)
        {
            GameObject.Destroy(this.gameObject);
        }

        this.currentTime += Time.deltaTime;

        this.gameObject.transform.localScale += new Vector3(.1f * scaleMultiplier, .1f * scaleMultiplier, .1f * scaleMultiplier);
    }
}

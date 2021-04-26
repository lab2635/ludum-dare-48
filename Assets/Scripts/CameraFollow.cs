using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float cameraDistance;
    public float zoomModifier;
    public float speed;

    private GameObject hookObject;

    // Start is called before the first frame update
    void Start()
    {
        this.cameraDistance *= -1;
        this.hookObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float zoomOffset = this.transform.position.y * zoomModifier;
        Vector3 cameraTargetPosition = new Vector3(this.hookObject.transform.position.x, this.hookObject.transform.position.y, this.hookObject.transform.position.z + this.cameraDistance + zoomOffset);
        this.transform.position = Vector3.MoveTowards(this.transform.position, cameraTargetPosition, speed * Time.deltaTime);
    }
}

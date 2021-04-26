using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool moveUp;
    public bool moveDown;
    public bool moveLeft;
    public bool moveRight;
    public bool yankLeft;
    public bool yankRight;
    public bool boost;
    public bool blast;
    public bool sonarPulse;
    public bool toggleHeadlight;
    public bool releaseHook;
    public bool accept;
    
    void Update()
    {
        accept = Input.anyKeyDown;
        moveLeft = Input.GetKey(KeyCode.A);
        moveRight = Input.GetKey(KeyCode.D);
        moveUp = Input.GetKey(KeyCode.W);
        moveDown = Input.GetKey(KeyCode.S);
        yankLeft = Input.GetKeyDown(KeyCode.A);
        yankRight = Input.GetKeyDown(KeyCode.D);
        boost = Input.GetKey(KeyCode.Space);
        blast = Input.GetKeyDown(KeyCode.Q);
        sonarPulse = Input.GetKeyDown(KeyCode.F);
        toggleHeadlight = Input.GetKeyDown(KeyCode.R);
        releaseHook = Input.GetKeyDown(KeyCode.E);
    }
}

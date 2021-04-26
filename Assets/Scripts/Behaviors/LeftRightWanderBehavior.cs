using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightWanderBehavior : FishBehavior
{
    public override Vector3 ChooseNewDestination(Vector3 currentPosition, Vector3 goal, float interactDistance, bool fleeing)
    {
        // Pick a position to the left or right and go there
        float distance = 2f;
        if (Random.value < 0.5f)
        {
            distance *= -1;
        }
        return currentPosition + new Vector3(distance, 0f);
    }
}

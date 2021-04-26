using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidGoalBehavior : FishBehavior
{
    public override Vector3 ChooseNewDestination(Vector3 currentPosition, Vector3 goal, float interactDistance, bool fleeing)
    {
        // Stay away from the hook
        Vector3 distance = currentPosition - goal;
        if (distance.magnitude <= interactDistance || fleeing)
        {
            return Flee(currentPosition, goal);
        }
        else
        {
            return Wander(currentPosition);
        }
    }
}

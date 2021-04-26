using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToGoalBehavior : FishBehavior
{
    public override Vector3 ChooseNewDestination(Vector3 currentPosition, Vector3 goal, float interactDistance, bool fleeing)
    {
        if (fleeing)
        {
            return Flee(currentPosition, goal);
        }

        Vector3 distance = currentPosition - goal;
        if (distance.magnitude <= interactDistance)
        {
            // Interact with the hook!
            // Basic AI: move toward the hook
            return goal;
        }
        else
        {
            return Wander(currentPosition);
        }
    }
}

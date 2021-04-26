using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSharkRoam : FishBehavior
{
    public override Vector3 ChooseNewDestination(Vector3 currentPosition, Vector3 goal, float interactDistance, bool fleeing)
    {
        if (fleeing)
        {
            return Flee(currentPosition, goal);
        }
        else
        {
            // Wander
            // Choose a vector
            float direction = Random.value * Mathf.PI * 2f;
            Vector3 newDirection = new Vector3((float)Mathf.Cos(direction), (float)Mathf.Sin(direction));

            // Go the distance
            float wanderDistance = Random.Range(15, 40);
            return currentPosition + newDirection.normalized * wanderDistance;
        }
    }
}

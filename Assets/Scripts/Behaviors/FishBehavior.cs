using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class FishBehavior
{
    public abstract Vector3 ChooseNewDestination(Vector3 currentPosition, Vector3 goal, float interactDistance, bool fleeing);

    public static FishBehavior GetBehavior(BehaviorType behaviorType)
    {
        switch(behaviorType)
        {
            case BehaviorType.WanderLeftRight:
                return new LeftRightWanderBehavior();
            case BehaviorType.MoveToGoal:
                return new MoveToGoalBehavior();
            case BehaviorType.AvoidGoal:
                return new AvoidGoalBehavior();
            case BehaviorType.GiantShark:
                return new GiantSharkRoam();

        }
        throw new ArgumentException("Bad behavior!", nameof(behaviorType));
    }

    protected Vector3 Wander(Vector3 currentPosition)
    {
        // Wander
        // Choose a vector
        float direction = UnityEngine.Random.value * Mathf.PI * 2f;
        Vector3 newDirection = new Vector3((float)Mathf.Cos(direction), (float)Mathf.Sin(direction));

        // Go the distance
        float wanderDistance = UnityEngine.Random.value * 5f;
        return currentPosition + newDirection.normalized * wanderDistance;
    }

    protected Vector3 Flee(Vector3 currentPosition, Vector3 goal)
    {
        return currentPosition + (currentPosition - goal);
    }
}

public enum BehaviorType
{
    WanderLeftRight,
    MoveToGoal,
    AvoidGoal,
    GiantShark
}
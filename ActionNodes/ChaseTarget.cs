using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChaseTarget : ActionNode
{
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;

    protected override void OnStart() {
        if (context.agent.hasPath && context.agent.destination == blackboard.moveToPosition)
            return;
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.destination = blackboard.moveToPosition;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
    }

    protected override void OnStop() {}

    protected override State OnUpdate()
    {
        if (context.agent.pathPending) {
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance) {
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid) {
            return State.Failure;
        }

        return State.Running;
    }

    public override void OnDrawGizmos() {
        if (Application.isPlaying && context.agent != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(context.agent.destination, 0.5f);
            Gizmos.DrawLine(context.agent.transform.position, context.agent.destination);
        }
    }
}

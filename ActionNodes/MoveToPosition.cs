using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveToPosition : ActionNode
{
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;
    public bool isInterruptable = false;

    protected override void OnStart() 
    {
        if (context.agent == null || !context.agent.enabled)
            return;
        if (context.agent.hasPath && context.agent.destination == blackboard.moveToPosition)
            return;
        if (blackboard.moveToPosition != Vector3.zero)
            context.agent.destination = blackboard.moveToPosition;
        else
        {
            Debug.Log("MoveToPosition: moveToPosition is zero");
            context.agent.ResetPath();
            return;
        }
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
    }

    protected override void OnStop() {}

    protected override State OnUpdate()
    {
        if (context.agent == null || !context.agent.enabled)
        {
            Debug.Log($"{context.gameObject.name}: MoveToPosition: agent is null or disabled");
            return State.Failure;
        }
        // if (context.agent.hasPath && context.agent.destination == blackboard.moveToPosition)
        //     return State.Success;

        if (context.agent.pathPending)
        {
            Debug.Log($"{context.gameObject.name}: MoveToPosition: agent path pending");
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance)
        {
            Debug.Log($"{context.gameObject.name}: MoveToPosition: agent remaining distance is less than tolerance");
            // blackboard.moveToPosition = Vector3.zero;
            // context.agent.ResetPath();
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            return State.Failure;

        if (isInterruptable)
        {
            Debug.Log($"{context.gameObject.name}: MoveToPosition: agent is interruptable");
            return State.Success;
        }
        else
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

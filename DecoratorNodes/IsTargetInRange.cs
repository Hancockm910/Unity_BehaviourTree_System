using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IsTargetInRange : DecoratorNode
{
    public float range = 10f;
    public bool requiresHealth = false;
    public bool requiresLineOfSight = false;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        // If there's no child, the outcome is irrelevant
        if (child == null)
            return State.Failure;
        // No target to search for
        if (blackboard.currentTargetObject == null)
            return State.Failure;
        // Check for health when the children depend on it
        if (requiresHealth)
        {
            var attributes = blackboard.GetHighestPriorityTarget().GetComponent<EntityAttributes>();
            if (attributes == null || attributes.GetCurrentHealth() <= 0)
            {
                blackboard.currentTargetObject = null;
                return State.Failure;
            }
        }
        // LoS check
        if (requiresLineOfSight)
        {
            var direction = (blackboard.GetHighestPriorityTarget().transform.position - context.agent.transform.position).normalized;
            var ray = new Ray(context.agent.transform.position, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, range))
            {
                if (hit.collider.gameObject != blackboard.currentTargetObject)
                {
                    return State.Failure;
                }
            }
        }
        // Check if the target is within range
        if (Vector3.Distance(context.agent.transform.position, blackboard.GetHighestPriorityTarget().transform.position) > range)
            return State.Failure;

        // If all checks pass, return the child's outcome
        // blackboard.moveToPosition = Vector3.zero;
        // context.agent.ResetPath();
        return child.Update();
    }
}

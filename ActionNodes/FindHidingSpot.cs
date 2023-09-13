using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FindHidingSpot : ActionNode
{
    public float threshold = 5.0f;
    public float tolerance = 1.0f;
    public int maxAttempts = 50;

    protected override void OnStart() 
    {
        if (context.agent == null || !context.agent.enabled)
        {
            Debug.Log($"{context.gameObject.name}: FindHidingSpot: agent is null or disabled");
            return;
        }

        Vector3 newPos = GetSafePosition();
        if (newPos != Vector3.zero)
        {
            context.agent.destination = newPos;
        }
    }
    private void DrawDebugCircle(Vector3 center, float radius, int numSegments, Color color)
    {
        float angle = 360.0f / numSegments;
        Vector3 lastPoint = center + new Vector3(radius, 0.0f, 0.0f);

        for (int i = 1; i <= numSegments; i++)
        {
            float x = center.x + radius * Mathf.Sin(angle * i * Mathf.Deg2Rad);
            float z = center.z + radius * Mathf.Cos(angle * i * Mathf.Deg2Rad);
            Vector3 newPoint = new Vector3(x, 0.0f, z);
            Debug.DrawLine(lastPoint, newPoint, color, 2.0f);
            lastPoint = newPoint;
        }
    }

    private Vector3 GetSafePosition()
    {
        int maxRadius = Mathf.CeilToInt(threshold);
        Vector3 currentPosition = context.transform.position;
        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();

        for (int radius = 1; radius <= maxRadius; radius++)
        {
            for (int angle = 0; angle < 360; angle += 10)
            {
                float radAngle = Mathf.Deg2Rad * angle;
                Vector3 targetPos = currentPosition + new Vector3(Mathf.Cos(radAngle), 0, Mathf.Sin(radAngle)) * radius;

                UnityEngine.AI.NavMeshHit hit;
                if (UnityEngine.AI.NavMesh.SamplePosition(targetPos, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
                {
                    // Check if there is a valid path to the location
                    if (UnityEngine.AI.NavMesh.CalculatePath(currentPosition, hit.position, UnityEngine.AI.NavMesh.AllAreas, path) && path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
                    {
                        // Draw debug line for test positions
                        Debug.DrawLine(currentPosition, hit.position, Color.yellow, 3.0f);

                        // Check if the position is out of line of sight by checking all layers
                        if (Physics.Linecast(hit.position, blackboard.GetHighestPriorityTarget().transform.position, out var hitInfo, -1))
                        {
                            // Draw debug ray for chosen position out of line of sight
                            Debug.DrawRay(currentPosition, (hit.position - currentPosition), Color.green, 3.0f);

                            blackboard.moveToPosition = hit.position;
                            return blackboard.moveToPosition;
                        }
                    }
                }
            }
        }

        // If no position is out of line of sight, move as far away from the target as possible
        Vector3 furthestDistancePosition = Vector3.zero;
        float maxDistance = float.MinValue;

        for (int angle = 0; angle < 360; angle += 10)
        {
            float radAngle = Mathf.Deg2Rad * angle;
            Vector3 targetPos = currentPosition + new Vector3(Mathf.Cos(radAngle), 0, Mathf.Sin(radAngle)) * maxRadius;

            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(targetPos, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                // Check if there is a valid path to the location
                if (UnityEngine.AI.NavMesh.CalculatePath(currentPosition, hit.position, UnityEngine.AI.NavMesh.AllAreas, path) && path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
                {
                    // Draw debug line for test positions
                    Debug.DrawLine(currentPosition, hit.position, Color.yellow, 3.0f);

                    float distance = Vector3.Distance(blackboard.GetHighestPriorityTarget().transform.position, hit.position);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        furthestDistancePosition = hit.position;
                    }
                }
            }
        }

        // Draw debug ray for chosen position furthest from the target
        Debug.DrawRay(currentPosition, (furthestDistancePosition - currentPosition), Color.red, 3.0f);

        blackboard.moveToPosition = furthestDistancePosition;
        return blackboard.moveToPosition;
    }



    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        if (context.agent == null || !context.agent.enabled)
        {
            Debug.Log($"{context.gameObject.name}: FindHidingSpot: agent is null or disabled");
            return State.Failure;
        }
        if (context.agent.pathPending)
        {
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance)
        {
            Debug.Log($"{context.gameObject.name}: FindHidingSpot: agent remaining distance is less than tolerance");
            // blackboard.moveToPosition = Vector3.zero;
            // context.agent.ResetPath();
            return State.Success;
        }
        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
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
            Gizmos.color = Color.yellow;
            if (blackboard.currentTargetObject != null)
                Gizmos.DrawLine(context.agent.transform.position, blackboard.GetHighestPriorityTarget().transform.position);
        }
    }
}

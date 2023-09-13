using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FindTargetInRangeOnLayer : DecoratorNode
{
    public LayerMask targetLayer;
    public bool requiresHealth = false;
    public bool requiresLineOfSight = false;
    public float range = 20f;
    public float fieldOfVision = 90f;

    protected override void OnStart()
    {
        blackboard.currentTargetObject = null;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        blackboard.currentTargetObject = null;
        // Get all colliders on the search layer.
        Collider[] colliders = Physics.OverlapSphere(context.agent.transform.position, range, targetLayer);
        // Debug.Log("Found " + colliders.Length + " colliders on layer " + targetLayer.value);
        // Draw a cone in front of the context object to represent the area that is being searched
        Vector3 startPos = context.agent.transform.position;
        Vector3 forward = context.agent.transform.forward;
        Vector3 endPos = startPos + forward * range;
        float line_angle = fieldOfVision / 2;
        Vector3 right = Quaternion.AngleAxis(line_angle, Vector3.up) * forward;
        Vector3 left = Quaternion.AngleAxis(-line_angle, Vector3.up) * forward;
        Debug.DrawLine(startPos, endPos, Color.green);
        Debug.DrawLine(startPos, startPos + right * range, Color.green);
        Debug.DrawLine(startPos, startPos + left * range, Color.green);
        // Find the collider that is closest to the origin.
        float closestDistance = float.MaxValue;
        Collider closestCollider = null;
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != null && collider.gameObject.activeSelf && collider.gameObject != context.agent.gameObject)
            {
                if (requiresHealth)
                {
                    var attributes = collider.gameObject.GetComponent<EntityAttributes>();
                    if (attributes == null)
                    {
                        // Debug.Log("Collider: " + collider.gameObject.name + " does not have EntityAttributes");
                        continue;
                    }
                    else if (attributes.GetCurrentHealth() <= 0)
                    {
                        // Debug.Log("Collider: " + collider.gameObject.name + " has 0 health");
                        continue;
                    }
                }
                if (requiresLineOfSight)
                {
                    var direction = (collider.transform.position - context.agent.transform.position).normalized;
                    var angle = Vector3.Angle(direction, context.agent.transform.forward);
                    if (angle > fieldOfVision / 2) // fieldOfVision is the variable that represents the field of vision
                    {
                        // Debug.Log("Angle: " + angle + " is greater than " + fieldOfVision / 2);
                        continue;
                    }

                    var ray = new Ray(context.agent.transform.position, direction);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, range))
                    {
                        if (hit.collider.gameObject != collider.gameObject)
                        {
                            
                            // Debug.Log("Raycast hit: " + hit.collider.gameObject.name + " instead of " + collider.gameObject.name);
                            continue;
                        }
                    }
                }
                float distance = Vector3.Distance(context.agent.transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCollider = collider;
                }
            }
        }

        // Print the name of the closest object.
        if (closestCollider != null)
        {
            blackboard.moveToPosition = closestCollider.transform.position;
            blackboard.currentTargetObject = closestCollider.gameObject;
            return child.Update();
        }
        else
        {
            Debug.Log("No objects found on the search layer within range.");
            return State.Failure;
        }
    }
}

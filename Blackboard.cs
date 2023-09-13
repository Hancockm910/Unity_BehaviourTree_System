using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Blackboard
{
    public Vector3 moveToPosition;
    public GameObject currentTargetObject;
    public Dictionary<GameObject, int> targetObjects = new Dictionary<GameObject, int>();
    public Dictionary<string, int> resources = new Dictionary<string, int>();

    // Add a method to update the priority of a target object
    public void UpdateTargetPriority(GameObject target, int priority)
    {
        if (targetObjects.ContainsKey(target))
        {
            targetObjects[target] = priority;
        }
        else
        {
            targetObjects.Add(target, priority);
        }
    }

    // Add a method to remove a target object from the dictionary
    public void RemoveTarget(GameObject target)
    {
        targetObjects.Remove(target);
    }

    public Dictionary<GameObject, int> GetTargetObjects()
    {
        return targetObjects;
    }

    // Add a method to get the target object with the highest priority
    public GameObject GetHighestPriorityTarget()
    {
        GameObject highestPriorityTarget = null;
        int highestPriority = int.MinValue;

        foreach (var pair in targetObjects)
        {
            if (pair.Key == null)
            {
                // Remove the target from the dictionary
                RemoveTarget(pair.Key);
                continue;
            }
            if (pair.Value > highestPriority)
            {
                highestPriority = pair.Value;
                highestPriorityTarget = pair.Key;
            }
        }

        return highestPriorityTarget;
    }
}

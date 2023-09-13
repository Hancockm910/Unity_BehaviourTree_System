using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelectPriorityTarget : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        // loop through targetObjects list from the blackboard and set the one with the highest priority to the targetObject
        // if there are no targetObjects, return failure
        // if (blackboard.currentTargetObjects.Count == 0)
        //     return State.Failure;
        
        // int highestPriority = 0;
        // GameObject highestPriorityTarget = null;
        // foreach (GameObject target in blackboard.currentTargetObjects)
        // {
        //     if (target != null && target.GetComponent<Priority>() != null)
        //     {
        //         if (target.GetComponent<Priority>().priority > highestPriority)
        //         {
        //             highestPriority = target.GetComponent<Priority>().priority;
        //             highestPriorityTarget = target;
        //         }
        //     }
        // }
        // if (highestPriorityTarget == null)
        //     return State.Failure;

        // blackboard.currentTargetObject = highestPriorityTarget;

        return State.Success;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaypointSelection : ActionNode {
    public int lastIndex = 0;


    protected override void OnStart() 
    {
    }

    protected override void OnStop() {}

    protected override State OnUpdate()
    {
        // if (blackboard.moveToPosition != Vector3.zero)
        // {
        //     Debug.Log($"WaypointSelection: moveToPosition = {blackboard.moveToPosition}");
        //     return State.Success;
        // }
        // if(context.agent.hasPath)
        // {
        //     Debug.Log($"WaypointSelection: agent has path");
        //     return State.Success;
        // }
        if (lastIndex + 1 < context.waypoints.waypointDestinations.Length)
        {
            // do something with the transform at the current index
            lastIndex++;
        }
        else
        {
            // if we have reached the end of the array, reset the index to 0
            lastIndex = 0;
        }
        Debug.Log($"WaypointSelection: lastIndex = {lastIndex}");
        blackboard.moveToPosition = context.waypoints.waypointDestinations[lastIndex].position;
        return State.Success;
    }
}

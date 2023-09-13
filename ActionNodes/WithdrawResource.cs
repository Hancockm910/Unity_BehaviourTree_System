using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WithdrawResource : ActionNode
{
    public int amount = 5;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        var collected = blackboard.GetHighestPriorityTarget().GetComponent<MineableResource>().Mine(amount);
        // Add the resource to the stockpile, or increase its quantity if it is already present

        if (blackboard.resources.ContainsKey(collected.Key))
        {
            blackboard.resources[collected.Key] += collected.Value;
        }
        else
        {
            blackboard.resources.Add(collected.Key, collected.Value);
        }

        return State.Success;
    }
}

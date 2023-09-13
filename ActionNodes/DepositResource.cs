using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DepositResource : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        blackboard.GetHighestPriorityTarget().GetComponent<Stockpile>().Deposit(blackboard.resources);
        // Clear the resources dictionary
        blackboard.resources.Clear();
        return State.Success;
    }
}

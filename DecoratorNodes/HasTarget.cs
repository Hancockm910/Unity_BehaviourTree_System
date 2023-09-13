using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HasTarget : DecoratorNode
{
    public bool invert = false;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        if (invert)
            return blackboard.currentTargetObject == null ? child.Update() : State.Failure;
        else
            return blackboard.currentTargetObject != null ? child.Update() : State.Failure;
    }
}

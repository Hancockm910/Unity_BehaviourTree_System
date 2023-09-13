using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HasMoveToPosition : DecoratorNode
{
    public bool invert = false;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        if (invert)
            return blackboard.moveToPosition == null ? child.Update() : State.Failure;
        return blackboard.moveToPosition != null ? child.Update() : State.Failure;    
    }
}

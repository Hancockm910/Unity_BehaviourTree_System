using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    A FallbackNode will cycle through it's children until one returns success.
*/
[System.Serializable]
public class FallbackNode : CompositeNode
{
    int current;

    protected override void OnStart()
    {
        current = 0;    
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (current >= children.Count)
            return State.Success;
        var child = children[current];
        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                current++;
                break;
            case State.Success:
                return State.Success;
        }

        return current == children.Count ? State.Success : State.Running;
    }
}

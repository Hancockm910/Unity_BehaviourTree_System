using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    A SequencerNode will cycle through it's children until one returns failure.
*/
[System.Serializable]
public class SequencerNode : CompositeNode
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
                return State.Failure;
            case State.Success:
                current++;
                break;
        }

        return current == children.Count ? State.Success : State.Running;
    }
}

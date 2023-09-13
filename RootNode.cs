using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RootNode : Node {

    [SerializeReference]
    [HideInInspector] 
    public Node child;
    public bool repeat = true;

    protected override void OnStart() {

    }

    protected override void OnStop() {

    }

    protected override State OnUpdate() {
        if (child != null)
        {
            var currentState = child.Update();
            if (repeat && (currentState == State.Failure || currentState == State.Success))
            {
                return State.Running;
            }
        }
        return State.Failure;
    }
}

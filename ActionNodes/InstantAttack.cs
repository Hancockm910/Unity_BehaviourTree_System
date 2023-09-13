using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstantAttack : ActionNode
{
    public int damage = 10;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        if (blackboard.currentTargetObject != null)
        {
            var targetsAttributes = blackboard.GetHighestPriorityTarget().GetComponent<EntityAttributes>();
            if (targetsAttributes != null && targetsAttributes.GetCurrentHealth() > 0)
            {
                targetsAttributes.TakeDamage(-damage);
                if (targetsAttributes.GetCurrentHealth() <= 0)
                {
                    blackboard.currentTargetObject = null;
                }
            }
            else
            {
                blackboard.currentTargetObject = null;
            }
        }
        return State.Success;
    }
}

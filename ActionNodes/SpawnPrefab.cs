using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPrefab : ActionNode
{
    public GameObject prefab;
    public Vector3 offset;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        if (blackboard.currentTargetObject == null)
        {
            if (prefab != null)
            {
                var spawnPosition = context.gameObject.transform.position + offset;
                var spawnRotation = context.gameObject.transform.rotation;
                var newObject = GameObject.Instantiate(prefab, spawnPosition, spawnRotation);
                blackboard.currentTargetObject = newObject;
            }
        }
        return State.Success;
    }
}

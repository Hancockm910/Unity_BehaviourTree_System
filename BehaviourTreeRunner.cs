using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    // Main tree
    public BehaviourTree tree;

    // Storage container object to hold game object subsystems
    Context context;

    // Start is called before the first frame update
    void Start()
    {
        if (tree)
        {
            context = CreateBehaviourTreeContext();
            if (context.animator != null)
                context.animator.enabled = true;
            if (context.agent != null)
                context.agent.enabled = true;
            foreach (CharacterJoint joint in context.joints)
            {
                joint.enableCollision = false;
            }
            foreach (Rigidbody rigidbody in context.rigidbodies)
            {
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
            }
            // Subscribe to the EntityDeath event using the entityAttributes field.
            // Create a method that will be called when the EntityDeath event is raised.
            void OnEntityDeath()
            {
                // Put your code here to handle the entity's death.
                Debug.Log($"{context.gameObject.name} has died!");
                if (context.animator != null)
                    context.animator.enabled = false;
                if (context.agent != null)
                    context.agent.enabled = false;
                foreach (CharacterJoint joint in context.joints)
                {
                    joint.enableCollision = true;
                }
                foreach (Rigidbody rigidbody in context.rigidbodies)
                {
                    // rigidbody.velocity = Vector3.zero;
                    rigidbody.detectCollisions = true;
                    rigidbody.useGravity = true;
                    rigidbody.isKinematic = false;
                    rigidbody.AddRelativeForce(Vector3.up * 10f, ForceMode.Impulse);
                    Vector3 torque = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                    rigidbody.AddRelativeTorque(torque, ForceMode.VelocityChange);
                }
                
                Destroy(context.gameObject, 5f);
            }
            if (context.entityAttributes != null)
                context.entityAttributes.EntityDeath += OnEntityDeath;
            tree = tree.Clone();
            tree.Bind(context);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tree)
            tree.Update();
    }

    // Initialize the context object
    Context CreateBehaviourTreeContext()
    {
        return Context.CreateFromGameObject(gameObject);
    }

    // Setup Gizmos for the tree
    private void OnDrawGizmosSelected()
    {
        if (!tree) {
            return;
        }

        BehaviourTree.Traverse(tree.rootNode, (n) =>
        {
            if (n.drawGizmos) {
                n.OnDrawGizmos();
            }
        });
    }
}

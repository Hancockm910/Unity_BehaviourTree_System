using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// The context is a shared object every node has access to.
// Commonly used components and subsytems should be stored here
public class Context
{
    public GameObject gameObject;
    public Transform transform;
    public Animator animator;
    public Rigidbody physics;
    public NavMeshAgent agent;
    public SphereCollider sphereCollider;
    public BoxCollider boxCollider;
    public CapsuleCollider capsuleCollider;
    public CharacterController characterController;
    public Waypoints waypoints;
    public EntityAttributes entityAttributes;
    public Rigidbody[] rigidbodies;
    public CharacterJoint[] joints;


    public static Context CreateFromGameObject(GameObject gameObject)
    {
        // Fetch all commonly used components
        Context context = new Context();
        context.gameObject = gameObject;
        context.transform = gameObject.transform;
        context.animator = gameObject.GetComponent<Animator>();
        context.physics = gameObject.GetComponent<Rigidbody>();
        context.agent = gameObject.GetComponent<NavMeshAgent>();
        context.sphereCollider = gameObject.GetComponent<SphereCollider>();
        context.boxCollider = gameObject.GetComponent<BoxCollider>();
        context.capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        context.characterController = gameObject.GetComponent<CharacterController>();
        context.waypoints = gameObject.GetComponent<Waypoints>();
        context.entityAttributes = gameObject.GetComponent<EntityAttributes>();
        context.rigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();
        context.joints = gameObject.GetComponentsInChildren<CharacterJoint>();

        return context;
    }
}

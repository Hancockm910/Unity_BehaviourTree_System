using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecoratorNode : Node {

    [SerializeReference]
    [HideInInspector] 
    public Node child;
}

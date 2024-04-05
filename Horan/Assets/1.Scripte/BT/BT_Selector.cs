using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Selector : BT_Node
{
    protected List<BT_Node> Child;
    public BT_Selector(List<BT_Node> child)
    {
        if (child != null)
            Child = child;
        
    }
    public override NodeState Evaluate()
    {
        if (Child == null)
        {
            State = NodeState.Failure;
            return NodeState.Failure;
        }

        foreach (BT_Node node in Child)
        {
            NodeState childState = node.Evaluate();
            switch (childState)
            {
                case NodeState.Success:
                    State = NodeState.Success;
                    return NodeState.Success;
                case NodeState.Running:
                    State = NodeState.Running;
                    return NodeState.Running;
            }
        }

        State = NodeState.Failure;
        return NodeState.Failure;
    }
}

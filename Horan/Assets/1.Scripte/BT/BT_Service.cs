using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Service : BT_Node
{
    protected BT_Node Child;

    public Action ServiceFun=null;

    MonsterGroup BTOwner;
    
    float Interval;
    float time;

    public BT_Service(BT_Node child, MonsterGroup owner, Action serviceFun, float interval=1)
    {
        if (child != null)
        {
            Child = child;
            BTOwner = owner;
            ServiceFun = serviceFun;
            Interval = interval;
            time = interval;
        }
    }
    public override NodeState Evaluate()
    {
        State = Child.Evaluate();

        if (BTOwner && State != NodeState.Failure)
        {
            BTOwner.Runner.OnService -= OnService;
            BTOwner.Runner.OnService += OnService;
        }

        return State;
    }
    void OnService()
    {
        State = Child.State;
        if (State == NodeState.Failure)
        {
            BTOwner.Runner.OnService -= OnService;
            return;
        }

        time += Time.deltaTime;
        if (Interval <= time)
        {
            if (ServiceFun != null)
                ServiceFun.Invoke();
            time = 0;
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Act
{
    public override void Init()
    {
        //���Ŀ� ���������̺��� �ε��ϴ� ������� �����ϱ� 
        ID = 1;
        AllowActs.Add(2);

    }
    public override void Execution()
    {
    }

    public override void Finish()
    {
    }


}

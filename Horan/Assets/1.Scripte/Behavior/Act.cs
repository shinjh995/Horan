using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KindOfAct //������ ���̺�� ���� �ϱ� 
{
    Attack=1,
}

public abstract class Act
{
    /*
     * �ൿ�� �����ϴ� ��ü 
     */

    protected int ID;
    protected List<int> AllowActs;
    //int[] BlockActs; //���� �ʿ���� 


    public abstract void Init();
    public abstract void Execution();
    public abstract void Finish();

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActComponent : MonoBehaviour
{
    /*
     * 
     * 
     * ������ü  + ����������Ʈ 
     * ������Ʈ�� �ൿ������ �������� ����
     * ���� �ش� ������Ʈ�� ���� ������Ʈ���� � ������ ������ �ϴ� ��� 
     * 
     * 
     */
    GameObject Actor; // ������
    bool isRunning; // ���� ���� ���� 
    int[] ActionableActs; // �����ڰ� ���� ������ ��� �������� ����
    //�� ������ �� �����ʹ� ���� ���������̺�� ������ ���� ���� ��ü�� ������ ���̺� �Ӽ����� �߰��� �ִ� �������� 

    Act CurAct; // ���� ���� ���� 
    int[] CurActionableAct; // ���� ������ ������ �մ� ������ 
    //int[] CurinfeasibleAct; // ���� ���� �Ұ����� ���� 

    public bool Execution()
    {
        bool canExecution = CanStart();
        if (canExecution)
        {
            
        }

        return false;
    }
    public bool Finish()
    {
        return false;
    }
    public bool CanStart()
    {
        return false;
    }

}

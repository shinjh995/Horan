using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGroup : MonoBehaviour,IDataBind
{

    /*
     * ������ ���۵Ǹ� ���� �׷���
     * 0.�ڽ��� ������ ���������� �������� ���� 
     * 1.������(����)���� ����
     * [�����Ϸ� ����]
     * 2.���������� ���¿� �´� �ൿ�� ��� 
     * 3.�������� ������ ������Ʈ �Ͽ� ������ ����� ȣ��
     */

    [SerializeField]
    int GroupId=1;
    
    List<MonsterController> MyGroup=new List<MonsterController>();

    public BTRunner Runner { get; protected set; }

    public void BindData()
    {
        string MonsterName;
        if (!Managers.DataLoder.DataCache_Groups.ContainsKey(GroupId)) return;

        for (int i = 0; i < Managers.DataLoder.DataCache_Groups[GroupId].member.Count; i++)
        {
            MonsterName= Managers.DataLoder.DataCache_Groups[GroupId].member[i].name;
            if (Managers.DataLoder.DataCache_Monsters.ContainsKey(MonsterName))
            {
                GameObject prefab = Resources.Load<GameObject>($"Monster/{MonsterName}");
                Object.Instantiate(prefab, transform);
                MonsterController addedMonster = prefab.GetComponent<MonsterController>();
                addedMonster.MyName = MonsterName;
                MyGroup.Add(addedMonster);
            }

        }
    } 
    private void Start()
    {
        BindData();

        // ���� ���� ��ġ ����
        // addedMonster.transform.position;

        /*  BT����
         *  3���� ��������
         *
         *  ������ �����ϱ�� ������ MonsterController�� ���Ͽ� ����� ������ �׽�ũ 
         *  
         *  �ൿ�� �� ģ���� ���� �ϴ� �׽�ũ(���� �������� 1���� ������ �������� ��� �ش� �����ڸ� �����ϰ� �ٸ� ���������� ������ �����ϵ��� �ϱ�)  
         *  
         *  ���� ��Ʈ�ѷ����� ���� void Attack()����� ���� ���͸��� ��ӽ��Ѽ� ���� ����
         *  ���ݽ� ���� ����ȸ�� ���� ���ӵ� �׽�ũ ���� ���� 
         *  
         *  �ᱹ �׷쿡 ���������� update���� �׻� �ൿ���� �����ϰ� �������̴� 
         *  � �ൿ�� ���� ������ BT���� �����ؼ� ȣ���ϴ°� 
         *  �׸��� �������� �ൿ�̳� ���°� ��ȭ�� �� �׷쿡 �˷��־���Ѵ�.
         *  
         *  ������ : ������ �ൿ�� �ش� ������ ��Ʈ�ѷ��� �����Ǿ� �ִ� 
         *  �׽�ũ�� ���Ͽ� � �޼��带 ȣ�������� �����ؾ� �Ѵ�.
         *  
         *  
         */

        Runner = new BTRunner
            (
                new BT_Selector
                (
                    new List<BT_Node>()
                    {
                        new BT_Task(RotateToAtkTarget)
                    }
                )
            );


    }
    private void Update()
    {
        if (Runner != null)
        {
            Runner.Operate();
            Runner.ServiceOperate();
        }
    }
    protected void StopUnit(bool isStop)
    {
        Runner.isActive = !isStop;
        //Nav.isStopped = isStop;
    }

    BT_Node.NodeState RotateToAtkTarget()
    {
        return BT_Node.NodeState.Success;
    }
    /*
     *  
     *  #region TaskFuns
    
    BT_Node.NodeState RotateToAtkTarget()
    {
        GameObject gameObject = FindCloseUnit(1 << 9);
        if (gameObject)
        {
            Vector3 dir = gameObject.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, dir) <= 45f / 2f) //���� �þ߰� 
            {
                return BT_Node.NodeState.Success;
            }
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 5*Time.deltaTime);
            return BT_Node.NodeState.Running;
        }
        return BT_Node.NodeState.Failure;
    }
    
    BT_Node.NodeState CheckAtkAnimPlaying()
    {
        if (IsAnimationRunning("ATTACK"))
        {
            return BT_Node.NodeState.Running;
        }

        return BT_Node.NodeState.Success;
    }

    BT_Node.NodeState Attack()
    {
        if (TargetPlayer != null)
        {       
            Anim.Play("ATTACK");
            return BT_Node.NodeState.Success;
        }   
        else
            return BT_Node.NodeState.Failure;
    }


    #endregion



 
     */

}

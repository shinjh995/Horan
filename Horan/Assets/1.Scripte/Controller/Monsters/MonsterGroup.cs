using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  BT����
 *  �׷��� ���Ǹ� ���� �Ÿ������� ���� �ൿ�ϴ� ���� ����
 *  ������ �ൿ�� ���ο��� ������ �ִ� ������ ���� ���� ���� 
 *  ���� �ൿ�� ������ �ٸ� �׷� ģ������ �ϳ��� BT�� ���� ���� ���ص� ��
 *
 *  ������ �����ϱ�� ������ MonsterController�� ���Ͽ� ����� ������ �׽�ũ 
 *  
 *  �ൿ�� �� ģ���� ���� �ϴ� �׽�ũ(���� �������� 1���� ������ �������� ��� �ش� �����ڸ� �����ϰ� �ٸ� ���������� ������ �����ϵ��� �ϱ�)  
 *  
 *  ���� ��Ʈ�ѷ����� ���� void Attack()����� ���� ���͸��� ��ӽ��Ѽ� ���� ����
 *  ���ݽ� ���� ����ȸ�� ���� ���ӵ� �׽�ũ ���� ���� 
 *  
 *  ���� �׷쵵 ������ ������ �ҵ� ?
 *  ���� ���� �ϴ°��� ���� �Ϲ� ���� �������� BT �ۼ� 
 *  
 */
public class MonsterGroup : MonoBehaviour,IDataBind
{
    [SerializeField]
    int GroupId=1;

    public BTRunner Runner { get; protected set; }

    List<MonsterController> MyGroup=new List<MonsterController>();
    [SerializeField]
    MonsterController ActableCtrl; // ���� �ൿ�� ������ ������
    [SerializeField]
    MonsterController CombatUnit; // ���� �������� ������
    [SerializeField]
    bool GroupCombat; // ���� ����

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
                GameObject go = Object.Instantiate(prefab, transform);

                MonsterController addedMonster = go.GetComponent<MonsterController>();
                addedMonster.MyName = MonsterName;
                MyGroup.Add(addedMonster);

            }
        }
    } 
    private void Start()
    {
        BindData();

        //�ﰢ���� ��ġ ����
        Vector3 center = transform.position;
        float angleIncrement = 360f / 3; // ���� 
        for (int i = 0; i < MyGroup.Count; i++)
        {
            float angle = i * angleIncrement; 
            float x = center.x + 1 * Mathf.Cos(Mathf.Deg2Rad * angle);
            float z = center.z + 1 * Mathf.Sin(Mathf.Deg2Rad * angle);
            MyGroup[i].transform.position = new Vector3(x, center.y, z); 
        }

        #region BT
        Runner = new BTRunner
            (
               new BT_Decorator(new BT_Service(new BT_Selector
                (
                    new List<BT_Node>()
                    {
                        new BT_Decorator(new BT_Sequence //���� ���
                        (
                            new List<BT_Node>()
                            {
                                new BT_Task(CombatWait)
                            }
                        ),IsGroupCombat),
                        new BT_Decorator(new BT_Selector //��,��
                        (
                           new List<BT_Node>()
                           {
                                new BT_Decorator(new BT_Selector //����, ���
                                (
                                    new List<BT_Node>()
                                    {
                                        new BT_Decorator(new BT_Task(Attack),RandomSelectAtkorGuard),
                                        new BT_Task(Guard) 
                                    }
                                ),IsCombatable)
                           }
                        ),IsPlayerInAtkRange),
                        new BT_Decorator(new BT_Sequence // Ž�� �Ÿ� �ȿ� �����ϴ� ���
                        (
                            new List<BT_Node>()
                            {
                                new BT_Task(Chase)
                            }
                        ),IsPlayerInSenseRange),
                        new BT_Sequence //����
                        (
                            new List<BT_Node>()
                            {
                                new BT_Task(Wandering)
                            }
                        )
                    }
                ), this, SetWanderingDestination, 7),SelectMember)
            );
        #endregion

    }
    private void Update()
    {
        if (Runner != null)
        {
            Runner.Operate();
            Runner.ServiceOperate();
        }
    }

    #region Task
    BT_Node.NodeState Wandering()
    {
        if (ActableCtrl.isActing)
            return BT_Node.NodeState.Running;

        ActableCtrl.Wandering(Runner);

        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState Attack()
    {
        if (ActableCtrl.isActing)
            return BT_Node.NodeState.Running;

        ActableCtrl.Attack(Runner);

        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState Chase()
    {
        if (ActableCtrl.isActing)
            return BT_Node.NodeState.Running;

        ActableCtrl.Chase(Runner);

        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState CombatWait()
    {
        if (ActableCtrl.isActing)
            return BT_Node.NodeState.Running;

        ActableCtrl.CombatWait(Runner);

        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState Guard()
    {
        Debug.Log("Guard");
        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState Exhaustion()
    {
        Debug.Log("Exhaustion");
        return BT_Node.NodeState.Success;
    }
    #endregion
    #region Service And Deco
    void SetWanderingDestination()
    {
        Vector3 pos = transform.position + new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
        for (int i = 0; i < MyGroup.Count; i++)
        {
            MyGroup[i].DestPos = pos;
        }
    }

    bool IsPlayerInAtkRange()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null|| ActableCtrl==null) return false;

        if (Vector3.Distance(ActableCtrl.transform.position ,player.transform.position) <= ActableCtrl.Stat.atkrange)
        {
            CombatUnit = ActableCtrl;
            GroupCombat = true;
            return true;
        }
        else
            return false;
    }
    bool IsPlayerInSenseRange()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null) return false;

        // 1�� �����ص� Ÿ�� on ������ 
        // ��� ���� ���ϸ� Ÿ�� off
        bool isSensing = false;
        for (int i = 0; i < MyGroup.Count; i++)
        {
            if (Vector3.Distance(MyGroup[i].transform.position, player.transform.position) <= MyGroup[i].Stat.sensingrange)
                isSensing = true;
        }

        if (isSensing)
        {
            for (int i = 0; i < MyGroup.Count; i++)
            {
                MyGroup[i].Target = player;
            }
            return true;
        }
 
        for (int i = 0; i < MyGroup.Count; i++)
        {
            MyGroup[i].Target = null;
        }
        CombatUnit = null;
        GroupCombat = false;
        return false;
    }
    bool SelectMember() // �ൿ�� ���������� ���� ����� ����
    {
        int count = 0;
        for (int i = 0; i < MyGroup.Count; i++) //�׷� ������ ������� ��� �׷� ���� 
        {
            if (!MyGroup[i].gameObject.activeSelf && MyGroup[i] == CombatUnit) 
            {
                count++;
                CombatUnit = null;
                GroupCombat = false;
            }
        }
        if (MyGroup.Count == count)
        {
            Runner.isActive = false;
            return false;
        }

        for (int i = 0; i < MyGroup.Count; i++)
        {
            if (MyGroup[i].isActing == false)
            {
                ActableCtrl = MyGroup[i];
                return true;
            }
        }

        ActableCtrl = null;

        return false;
    }
    bool RandomSelectAtkorGuard() // ����or��� ���� ����
    {
        return true;
    }
    bool IsCombatable() //�������� ��Ȳ ���� �Ǵ� 
    {
        if (ActableCtrl.Target == null) return false; // �÷��̾� �� ���� ��Ȳ  

        if (IsGroupCombat()) return false;

        if (CombatUnit == ActableCtrl)
            return true;
        
        return false;
    }
    bool IsGroupCombat() 
    {
        if (GroupCombat && ActableCtrl != CombatUnit )
            return true;
        else
            return false;
    }
    #endregion



}

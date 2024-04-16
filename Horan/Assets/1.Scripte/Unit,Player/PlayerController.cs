using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerAnimState
{
    IDLE,
    MOVE,
    DASH
}


public class PlayerController : UnitController
{
    Animator anim;
    PlayerStat stat;
    ActComponent Act;

    Weapon equippedWeapon;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        input = GetComponent<InputComponent>();
        input.MouseAction -= OnPlayerMouseEvent;
        input.MouseAction += OnPlayerMouseEvent;
        input.KeyAction -= OnPlayerKeyBoardEvent;
        input.KeyAction += OnPlayerKeyBoardEvent;

        move = GetComponent<MoveComponent>();
        stat = GetComponent<PlayerStat>();
        stat.OnHit += () => isCounter = true;
    }
    private void Start()
    {
        #region Acts 
        Act attack = new Act((int)KindOfAct.Attack, Attack);
        attack.AddAllowActID((int)KindOfAct.Attack);
        attack.AddAllowActID((int)KindOfAct.Guard);
        attack.AddAllowActID((int)KindOfAct.Dash);

        Act move = new Act((int)KindOfAct.Move, Move);
        move.AddAllowActID((int)KindOfAct.Dash);
        move.AddAllowActID((int)KindOfAct.Move);

        Act guard = new Act((int)KindOfAct.Guard, Guard);
        guard.AddAllowActID((int)KindOfAct.Attack);
        guard.AddAllowActID((int)KindOfAct.Counter);

        Act dash = new Act((int)KindOfAct.Dash, Dash);
        dash.AddAllowActID((int)KindOfAct.Dash);
        dash.AddAllowActID((int)KindOfAct.DashAttack);

        Act dashAtttack = new Act((int)KindOfAct.DashAttack, DashAttack);

        Act counter = new Act((int)KindOfAct.Counter, Counter);

        //�ൿ ���
        Act = GetComponent<ActComponent>();
        Act.AddAct(attack);
        Act.AddAct(move);
        Act.AddAct(guard);
        Act.AddAct(dash);
        Act.AddAct(dashAtttack);
        Act.AddAct(counter);
        //�ൿ ���� �� ���� ȣ��
        //Act.Execution((int)KindOfAct.Attack);
        //Act.Finish((int)KindOfAct.Attack);

        #endregion
        
        //���� ���� �ڵ�
        equippedWeapon = GetComponentInChildren<Weapon>();
    }
    Vector3 moveDir = Vector3.zero;
    public GameObject TargetEnemy=null;



    #region Input
    InputComponent input;
    void OnPlayerMouseEvent(InputComponent.MouseEvent evt)
    {
        switch (evt)
        {
            case InputComponent.MouseEvent.None:
                {
                }
                break;
            case InputComponent.MouseEvent.Press:
                {

                    // Debug.Log("Press");
                }
                break;
            case InputComponent.MouseEvent.PointerDown:
                {
                    // Debug.Log("PointerDown");
                }
                break;
            case InputComponent.MouseEvent.PointerUp:
                {
                    // Debug.Log("PointerUp");
                }
                break;
            case InputComponent.MouseEvent.Click:
                {
                    if (atkAble && atkCount < equippedWeapon.AttackAnimNames.Length)
                    {
                        Act.Execution((int)KindOfAct.Attack);
                        DashAtkInput = true;
                    }//Debug.Log("Click");
                }
                break;
            default:
                break;
        }
    }
    void OnPlayerKeyBoardEvent(InputComponent.KeyBoardEvent evt)
    {
        switch (evt)
        {
            case InputComponent.KeyBoardEvent.None:
                {
                    move.SetMove(0);
                    anim.SetInteger("AnimState", (int)EPlayerAnimState.IDLE);
                    Act.Finish((int)KindOfAct.Move);
                }
                break;
            case InputComponent.KeyBoardEvent.Press:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    Act.Execution((int)KindOfAct.Move);
                }
                if (Input.GetKey(KeyCode.E)&& !isGuard)
                {
                    Act.Execution((int)KindOfAct.Guard);
                }
                break;
            case InputComponent.KeyBoardEvent.ButtonDown:
                if (Input.GetKey(KeyCode.Space) && 0 < DashCount && stat.UseSP(20))
                    Act.Execution((int)KindOfAct.Dash);
                break;
            case InputComponent.KeyBoardEvent.ButtonUp:
                if (isGuard && !Input.GetKeyDown(KeyCode.E))
                {
                    isGuard = false;
                    Act.Finish((int)KindOfAct.Guard);
                }
                break;
            default:
                break;
        }
    }
    #endregion

    #region Move
    MoveComponent move; 
    void Move()
    {
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.z = Input.GetAxis("Vertical");

        if (TargetEnemy)
        {
            Vector3 targetDir = (TargetEnemy.transform.position - transform.position).normalized;
            move.SetMove(moveDir, targetDir, stat.speed);
        }
        else
            move.SetMove(moveDir, moveDir, stat.speed);

        anim.SetInteger("AnimState", (int)EPlayerAnimState.MOVE);
        anim.SetFloat("WalkX", moveDir.x);
        anim.SetFloat("WalkY", moveDir.z);
    }
    #endregion

    #region Attack

    bool atkAble = true;
    int atkCount = 0;

    void Attack()
    {
        StopCoroutine("ATTACK");
        StartCoroutine("ATTACK");
    }
    IEnumerator ATTACK()
    {
        if (atkCount < equippedWeapon.AttackAnimNames.Length)
        {
            anim.Play(equippedWeapon.AttackAnimNames[atkCount], -1, 0);
            atkCount += 1;
            atkAble = false;
        }
        else
        {
            atkCount = 0;
            Act.Finish((int)KindOfAct.Attack);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f); // ���� Ȱ��ȭ equippedWeapon.AtkDelayTimes[atkCount]
        equippedWeapon.Area.enabled = true;
        yield return new WaitForSeconds(0.3f); // ���� ��Ȱ��ȭ 
        equippedWeapon.Area.enabled = false;

        atkAble = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - 0.8f); //curAnimStateInfo.length - (equippedWeapon.AtkDelayTimes[atkCount]+0.3f)
        atkCount = 0;
        Act.Finish((int)KindOfAct.Attack);
    }

    #endregion

    #region Guard

    bool isGuard;
    public bool isCounter; //�ش� ������ stat���� �����ؼ� �����ؾ� ��
    const float GuardSuccessTime = 0.3f;

    void Guard()
    {
        StartCoroutine("GUARD");
    }
    IEnumerator GUARD()
    {
        isGuard = true;
        anim.Play("GUARD");
        anim.SetBool("GuardEnd", false);

        stat.isDamageable = false;

        isCounter = false;
        yield return new WaitForSeconds(GuardSuccessTime); //�ش� �ð��ȿ� ������ ���ý� ī���� ���� (�� �ð��� �Ƹ� ����ִϸ��̼� �����ð��̵ɵ�?)
        if (isCounter)
        {
            Act.Execution((int)KindOfAct.Counter);
            yield return null;
        }

        yield return new WaitUntil(() => !isGuard);
        isGuard = false;
        anim.SetBool("GuardEnd", true);

        stat.isDamageable = true;
    }

    void Counter()
    {
        StartCoroutine("COUNTER");
    }
    IEnumerator COUNTER()
    {
        anim.SetBool("GuardEnd", true);

        anim.Play("COUNTER");

        equippedWeapon.Area.enabled = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // ���� Ȱ��ȭ 
        equippedWeapon.Area.enabled = false;

        isGuard = false;

        stat.isDamageable = true;
       
        Act.Finish((int)KindOfAct.Counter);
    }


    #endregion

    #region Dash

    const float DashCoolTime = 3.0f;
    [SerializeField]
    float DashCount = 1;
    void Dash()
    {
        Debug.Log(stat.sp);
        StopCoroutine("DASH");
        StartCoroutine("DASH");
    }
    IEnumerator DASH()
    {
        DashCount -= 1;
        
        stat.isDamageable = false;

        move.MoveActive = false;
        move.MoveByPower(transform.forward, 10);
        
        anim.Play("DASH");

        DashAtkInput = false;
        yield return new WaitForSeconds(0.2f);
        if (DashAtkInput)
        {
            Act.Execution((int)KindOfAct.DashAttack);
            yield return null;
        }


        yield return new WaitForSeconds(0.2f); //�ִϸ��̼� ��� 
        stat.isDamageable = false;
        move.MoveActive = true;
        Act.Finish((int)KindOfAct.Dash);

        yield return new WaitForSeconds(DashCoolTime);
        DashCount += 1;
    }

    bool DashAtkInput; 
    void DashAttack()
    {
        StartCoroutine("DASHATTACK");
    }
    IEnumerator DASHATTACK()
    {
        anim.Play("DASHATTACK");
        equippedWeapon.Area.enabled = true;
        yield return new WaitForSeconds(0.3f); // ���� Ȱ��ȭ 
        equippedWeapon.Area.enabled = false;

        yield return new WaitForSeconds(0.7f);//�ִϸ��̼� ����
        Act.Finish((int)KindOfAct.DashAttack);
    }
    #endregion

    #region Loot&Equip
    void Loot()
    {
        //���� �ִϸ��̼� ����  + ��������->������ ����->�κ��丮 ���� + ������ ���� �ڽ� �ʿ� 
    }
    void Equip()
    {
        //�κ��丮UI���� ������ ������ �� �Լ� ȣ���  �ش� �������� ����   
        //�κ��丮 ��� ���� �ʿ� 
    }
    void WeaponEquip(Weapon weapon)
    {
        //���� ������ ����ϰ� �ִ� ��� �����Ѵٸ� ������ ��� ��Ȱ��ȭ ���� �����ϴ� �ڵ��ʿ�
    }
    #endregion

}
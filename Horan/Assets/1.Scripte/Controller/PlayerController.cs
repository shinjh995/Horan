using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UnitController
{
    Animator anim;
    public enum EPlayerAnimState
    {
        IDLE,
        MOVE,
        DASH
    }
    EPlayerAnimState ePlayerAnimState;
    public EPlayerAnimState AnimState 
    { 
        get { return ePlayerAnimState; }
        set 
        { 
            ePlayerAnimState = value;
            if (anim)
            {
                anim.SetInteger("AnimState", (int)ePlayerAnimState);
                switch (ePlayerAnimState)
                {
                    case EPlayerAnimState.IDLE:
                        move.SetMoveDir(Vector3.zero, 0);
                        break;
                    case EPlayerAnimState.MOVE:
                        float hor = Input.GetAxis("Horizontal"); //ad
                        float ver = Input.GetAxis("Vertical"); //ws
                        move.SetMoveDir(new Vector3(hor, 0, ver), stat.speed);
                        anim.SetFloat("WalkX", hor);
                        anim.SetFloat("WalkY", ver);
                        break;
                    case EPlayerAnimState.DASH:
                        break;
                }
               
            }
        } 
    }

    PlayerStat stat;

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
        stat.OnHit += () => { isHit = true; };
    }
    private void Start()
    {
        equippedWeapon = GetComponentInChildren<Weapon>();
      
        AnimState = EPlayerAnimState.IDLE;
    }


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
                    Attack();
                    //Debug.Log("Click");
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
                AnimState = EPlayerAnimState.IDLE;
                break;
            case InputComponent.KeyBoardEvent.Press:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                    AnimState = EPlayerAnimState.MOVE;
                if (Input.GetKey(KeyCode.E))
                {
                    Guard();
                }
                break;
            case InputComponent.KeyBoardEvent.ButtonDown:
                if (Input.GetKey(KeyCode.Space))
                    Dash();
                break;
            case InputComponent.KeyBoardEvent.ButtonUp:
                if (isGuard && !Input.GetKeyDown(KeyCode.E))
                {
                    isGuard = false;
                }
                break;
            default:
                break;
        }
    }
    #endregion

    #region Move
    MoveComponent move;

    #endregion

    #region Attack

    Weapon equippedWeapon;

    public Action OnAttackStart;
    public Action OnAttackEnd;

    int atkCount;
    bool atkAble=true;

    public void Attack()
    {
        if (equippedWeapon == null) return;
        if (equippedWeapon.AttackNames.Length <= atkCount) return;
        string atkName = equippedWeapon.AttackNames[atkCount];
        if (atkName == null) return;

        if (atkAble)
        {
            atkCount += 1;
            StopCoroutine("ATTACK");
            StartCoroutine("ATTACK", atkName);
        }
        //equippedWeapon.AttackNames[atkCount]
    }
    IEnumerator ATTACK(string atkName) //�ش� �Ű������� atkinfo���� ����ü �������� �����ص� ������ ����
    {
        if (OnAttackStart != null)
            OnAttackStart.Invoke();

        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(atkName, -1, 0);
        atkAble = false;

        yield return new WaitForSeconds(0.3f); // ��� �� ������

        equippedWeapon.Area.enabled = true;
        yield return new WaitForSeconds(0.3f); // ���� ���� 
        equippedWeapon.Area.enabled = false;

        yield return new WaitForSeconds(0.25f); //���� ���� ����
        atkAble = true;

        yield return new WaitForSeconds(curAnimStateInfo.length-0.85f);   // ���� ��� ���� ����
        atkCount = 0;

        if (OnAttackEnd != null)
            OnAttackEnd.Invoke();
    }
    
    #endregion

    #region Guard

    bool isGuard;
    public bool isHit; 
    float GuardSuccessTime=0.3f;

    void Guard()
    {
        if (!isGuard)
        {
            isGuard = true;
            StartCoroutine("GUARD");
        }
    }
    IEnumerator GUARD()
    {
        isHit = false;
        stat.isDamageable = !isGuard;
        anim.SetBool("GuardEnd", false);
        anim.Play("GUARD");
        yield return new WaitForSeconds(GuardSuccessTime); //�ش� �ð��ȿ� ������ ���ý� ī���� ���� (�� �ð��� �Ƹ� ����ִϸ��̼� �����ð��̵ɵ�?)

        //while() ���� ������ �� �������� ���� �����ϰ� �ֵ��� �ҿ��� ������ ������ 
        if (isHit)
        {
            stat.isDamageable = true;
            anim.SetBool("GuardEnd", true);
            StartCoroutine("ATTACK", "COUNTER");
        }

        yield return new WaitUntil(() => !isGuard);
        anim.SetBool("GuardEnd",true);
        stat.isDamageable = !isGuard;
    }

  
    #endregion

    #region Dash

    const float DashCoolTime=3.0f;
    [SerializeField]
    float DashCount = 1;
    bool isDash;
    void Dash()
    {
        //���� �߰� ���� 
        if  (0 < DashCount&&!isDash)
        {
            StartCoroutine("DASH");
        }
    }
    IEnumerator DASH()
    {
        isDash = true;
        DashCount -= 1;
        move.MoveActive = false;

        anim.Play("DASH");      
        move.MoveByPower(transform.forward,10);
        yield return new WaitForSeconds(0.4f); //�ִϸ��̼� ��� 
   
        isDash = false;
        move.MoveActive = true;
   
        yield return new WaitForSeconds(DashCoolTime);
 
        DashCount += 1;
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

    //�ൿ ���� ����ȭ ����� �ϱ� 
    //���� ���� ���� 
}

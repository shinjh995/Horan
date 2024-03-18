using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //������ ���̺� ���� ����
    public int damage;
    public string[] AttackNames;
    public float[] AtkDelayTime;

    GameObject Owner;
    public BoxCollider Area { get; private set; }



    private void Start()
    {
        //���� �����͵� ���̺��� �ҷ����� ������� �����ϱ� 
        damage = 10;
        Area = GetComponent<BoxCollider>();
        Owner = GetComponentInParent<Stat>().gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject hitob = other.gameObject;
        if (hitob)
        {
            IDamageInteraction damageable= hitob.GetComponent<IDamageInteraction>();
            if(damageable!=null)
                damageable.TakeDamage(damage);
        }
    }

}

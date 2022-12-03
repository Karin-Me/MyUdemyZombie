using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTools : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public bool DoesGatherResources;
    public bool doesdealDamage;
    public int damage;

    private Animator itemAnim;
    private Camera cam;

    private void Awake()
    {
        itemAnim = GetComponent<Animator>();
        cam = Camera.main;
    }

    public override void OnAttackInput()
    {
        attacking = true;
        itemAnim.SetTrigger("Attack");
        Invoke("OnCanAttack", attackRate);
    }

    void OnCanAttack()
    {
        attacking = true;
    }

    public void OnHit()
    {

    }
}

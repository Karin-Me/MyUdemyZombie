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
    [Header("Ranged Weapon")]
    public bool pistolType;
    public bool assaultType;
    public GameObject muzzle;   // ÃÑ±¸
    public Transform muzzlePoint;
    public AudioClip shotSound;
    private AudioSource audios;

    private Animator itemAnim;
    private Camera cam;
    public Sprite weaponSprite;

    private void Awake()
    {
        itemAnim = GetComponent<Animator>();
        cam = Camera.main;
    }

    public override void OnAttackInput()
    {
        if (!attacking)
        {

            attacking = true;
            itemAnim.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);


           
        }
        
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        Debug.Log("Hit");
    }
}

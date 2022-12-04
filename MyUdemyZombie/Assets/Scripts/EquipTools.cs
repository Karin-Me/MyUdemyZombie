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
        audios = GetComponent<AudioSource>();
    }



    private void Start()
    {
        PlayerPrefs.GetFloat("CurrentPistolAmmo");
        PlayerPrefs.GetFloat("CurrentAssaultAmmo");
    }



    public override void OnAttackInput()
    {
        if (!attacking && !pistolType && !assaultType)
        {
            attacking = true;
            itemAnim.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
        }
        else if (!attacking && pistolType && AmmoManager.instance.curPistolAmmo > 0)
        {
            attacking = true;
            itemAnim.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
            GameObject obj = Instantiate(muzzle, muzzlePoint.transform.position,
                                        muzzlePoint.transform.rotation * Quaternion.Euler(90, 0, 0));
            Destroy(obj, 0.1f);
            audios.PlayOneShot(shotSound);

            AmmoManager.instance.curPistolAmmo--;
            PlayerPrefs.SetFloat("CurrentPistolAmmo", AmmoManager.instance.curPistolAmmo);
        }

        else if (!attacking && assaultType && AmmoManager.instance.curAssaultAmmo > 0)
        {
            attacking = true;
            itemAnim.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
            GameObject obj = Instantiate(muzzle, muzzlePoint.transform.position,
                                        muzzlePoint.transform.rotation * Quaternion.Euler(90, 0, 0));
            Destroy(obj, 0.1f);
            audios.PlayOneShot(shotSound);
            // curAmmo--;
            AmmoManager.instance.curAssaultAmmo--;
            PlayerPrefs.SetFloat("CurrentAssaultAmmo", AmmoManager.instance.curAssaultAmmo);
        }


    }

    public void PistolReload(float amount)
    {
        if (pistolType)
        {
            AmmoManager.instance.ReloadPistol(amount);
            PlayerPrefs.SetFloat("CurrentPistolAmmo", AmmoManager.instance.curPistolAmmo);
        }

    }

    public void AssaultReload(float amount)
    {
        if (assaultType)
        {
            AmmoManager.instance.ReloadAssault(amount);
            PlayerPrefs.SetFloat("CurrentAssaultAmmo", AmmoManager.instance.curPistolAmmo);
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

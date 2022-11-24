using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public int damage;
    public float damageRate;
    private List<IDamageable> thingsToGetDamage = new List<IDamageable>();

    private void Start()
    {
        StartCoroutine(DoDamage());
    }

    IEnumerator DoDamage()
    {
        while(true)
        {
            for(int i = 0; i < thingsToGetDamage.Count; i++)
            {
                thingsToGetDamage[i].TakeDamage(damage);
            }

            yield return new WaitForSeconds(damageRate);
        }

        //yield return new WaitForSeconds(damageRate);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            thingsToGetDamage.Add(collision.gameObject.GetComponent<IDamageable>());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            thingsToGetDamage.Remove(collision.gameObject.GetComponent<IDamageable>());
        }
    }
}

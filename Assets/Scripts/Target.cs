using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
    public float dieDelay = 3f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    IEnumerator DelayedDie(float time)
    {
        foreach (Rigidbody rg in GetComponentsInChildren<Rigidbody>())
        {
            rg.isKinematic = false;
        }
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    void Die()
    {
        StartCoroutine(DelayedDie(dieDelay));
    }
}

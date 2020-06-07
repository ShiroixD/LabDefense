using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
    public float dieDelay = 3f;
    public Destructible destructible;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            if (destructible != null)
                destructible.Destroy();
        }
    }
}

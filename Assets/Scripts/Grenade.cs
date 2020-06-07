using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 3f;
    public float radius = 5f;
    public float force = 700f;

    public GameObject explosionEffect;

    private float countdown;
    private bool hasExploded = false;

    void Start()
    {
        countdown = delay;
        GetComponent<AudioSource>().PlayDelayed(2);
    }


    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        Vector3 position = transform.position;
        position.y += 2f;
        Instantiate(explosionEffect, position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }

            Target target = nearbyObject.GetComponent<Target>();
            if (target != null)
                target.TakeDamage(1000);
        }

        GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 2f);
    }
}

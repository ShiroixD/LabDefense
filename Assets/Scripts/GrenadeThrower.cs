using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public float throwForce = 40f;
    public GameObject grenadePrefab;
    public GameObject boxesPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ThrowGrenade();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Vector3 boxPos = transform.position + transform.forward * 10; ;
            Instantiate(boxesPrefab, boxPos, transform.rotation);
        }
    }

    void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
    }
}

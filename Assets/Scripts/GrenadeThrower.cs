using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public float throwForce = 40f;
    public GameObject grenadePrefab;
    public GameObject boxesPrefab;

    [SerializeField]
    private int _grenadeAmount;

    [SerializeField]
    private TextMeshProUGUI _grenadeCounter;

    void Start()
    {
        CalculateGrenadeCounter();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (_grenadeAmount > 0)
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
        _grenadeAmount--;
        CalculateGrenadeCounter();
    }

    private void CalculateGrenadeCounter()
    {
        _grenadeCounter.text = _grenadeAmount.ToString();
    }
}

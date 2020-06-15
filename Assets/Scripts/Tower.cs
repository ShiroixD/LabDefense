using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private Healthbar _healthBar;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        _healthBar.TakeDamage(amount);
    }

    public void GainHealth(int amount)
    {
        _healthBar.GainHealth(amount);
    }

    public void StartRegenerating()
    {
        _healthBar.regenerateHealth = true;
    }

    public void StopRegenerating()
    {
        _healthBar.regenerateHealth = false;
    }
}

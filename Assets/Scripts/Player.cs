using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStates State = PlayerStates.NORMAL;

    [SerializeField]
    private Healthbar _healthBar;

    void Start()
    {
        
    }

    void Update()
    {
        if (State == PlayerStates.PARALIZED && _healthBar.health == _healthBar.maximumHealth)
        {
            State = PlayerStates.NORMAL;
            GetComponent<Rigidbody>().isKinematic = false;
            _healthBar.healthPerSecond /= 3;
        }
    }

    public void TakeDamage(int amount)
    {
        _healthBar.TakeDamage(amount);
        if (_healthBar.health <= _healthBar.minimumHealth)
        {
            State = PlayerStates.PARALIZED;
            GetComponent<Rigidbody>().isKinematic = true;
            _healthBar.healthPerSecond *= 3;
        }
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

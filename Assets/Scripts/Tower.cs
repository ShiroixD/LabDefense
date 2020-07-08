using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private GameObject _startTransition;

    [SerializeField]
    private GameObject _endTransition;

    [SerializeField]
    private GameObject _winAnim;

    [SerializeField]
    private GameObject _gameOverAnim;

    [SerializeField]
    private Healthbar _healthBar;

    private bool _gameEnded = false;

    void Start()
    {
        StartCoroutine(StartGameTransition(1.5f));
    }

    void Update()
    {
        if (_healthBar.health <= 0)
        {
            GameOver();
        }
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

    public void GameOver()
    {
        StartCoroutine(GameOverTransition(1.5f));
    }

    public void Win()
    {
        StartCoroutine(WinTransition(1.5f));
    }

    private IEnumerator StartGameTransition(float time)
    {
        yield return new WaitForSeconds(time);
        _startTransition.SetActive(false);
    }

    private IEnumerator GameOverTransition(float time)
    {
        _gameEnded = true;
        _gameOverAnim.SetActive(true);
        yield return new WaitForSeconds(time);
        _endTransition.SetActive(true);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    private IEnumerator WinTransition(float time)
    {
        _gameEnded = true;
        _winAnim.SetActive(true);
        yield return new WaitForSeconds(time);
        _endTransition.SetActive(true);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}

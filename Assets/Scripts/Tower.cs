using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tower : MonoBehaviour
{
    public float timeLimitSec = 180;
    public float currentTimeSec;

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

    [SerializeField]
    private Spawner _spawner;

    [SerializeField]
    private TextMeshProUGUI _timeCounter;

    [SerializeField]
    private AudioSource _backgroundAudio;

    [SerializeField]
    private AudioClip _gameOverMusic;

    [SerializeField]
    private AudioClip _winMusic;

    private bool _gameEnded = false;
    private bool halfGamePart = false;

    void Start()
    {
        currentTimeSec = timeLimitSec;
        StartCoroutine(StartGameTransition(1.5f));
    }

    void Update()
    {
        if (!_gameEnded)
            currentTimeSec -= Time.deltaTime;

        CalculateTimeCounter();

        if (_healthBar.health <= 0)
        {
            GameOver();
        }

        if (!halfGamePart && currentTimeSec <= timeLimitSec / 2.0f)
        {
            halfGamePart = true;
            _spawner.DoubleSpawnAmount();
        }

        if (currentTimeSec <= 0)
        {
            Win();
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
        _backgroundAudio.PlayOneShot(_gameOverMusic);
        StartCoroutine(GameOverTransition(1.5f));
    }

    public void Win()
    {
        _backgroundAudio.PlayOneShot(_winMusic);
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
        _spawner.spawnActive = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Target>().Disappear();
        }
        _gameOverAnim.SetActive(true);
        yield return new WaitForSeconds(time);
        _endTransition.SetActive(true);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    private IEnumerator WinTransition(float time)
    {
        _gameEnded = true;
        _spawner.spawnActive = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Target");
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<Target>().Disappear();
        }
        _winAnim.SetActive(true);
        yield return new WaitForSeconds(time);
        _endTransition.SetActive(true);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    private void CalculateTimeCounter()
    {
        string timeLabel = "";
        if (currentTimeSec <= 0)
            timeLabel += "00:00";
        else
        {
            int minutes = (int)(currentTimeSec / 60.0f);
            int seconds = (int)(currentTimeSec - minutes * 60);
            if (minutes < 10)
                timeLabel += "0";
            timeLabel += minutes.ToString() + ":";
            if (seconds < 10)
                timeLabel += "0";
            timeLabel += seconds.ToString();
        }
        _timeCounter.text = timeLabel;
    }
}

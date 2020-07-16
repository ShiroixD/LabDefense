using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private GameObject _transition;
    private bool _startedGame = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void BeginGame()
    {
        if (!_startedGame)
        {
            StartCoroutine(LoadGame(1.5f));
        }
    }

    private IEnumerator LoadGame(float time)
    {
        _startedGame = true;
        _transition.SetActive(true);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
}

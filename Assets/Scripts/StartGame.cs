using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private GameObject _transition;
    [SerializeField]
    private GameObject[] _objectsToEnable;
    private bool _startedGame = false;

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

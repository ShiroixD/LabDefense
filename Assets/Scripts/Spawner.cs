using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _spawnPoints;

    [SerializeField]
    private GameObject[] _enemyPrefabs;

    [SerializeField]
    private int _spawnAmount;

    [SerializeField]
    private int _spawnAddAmount;

    [SerializeField]
    private float _spawnDelay;

    [SerializeField]
    private AudioSource _enemySpawnAudio;

    public bool spawnActive = true;

    void Start()
    {
        StartCoroutine(Spawn(_spawnDelay));
    }

    void Update()
    {
        
    }
    
    public void StopSpawn()
    {
        spawnActive = false;
    }

    public void DoubleSpawnAmount()
    {
        _spawnAmount += 3;
    }

    private IEnumerator Spawn(float time)
    {
        yield return new WaitForSeconds(10);
        do
        {
            _enemySpawnAudio.Play();
            for (int i = 0; i < _spawnAmount; i++)
            {
                int spawnPointNumber = Random.Range(0, _spawnPoints.Length - 1);
                int enemyPrefabNumber = Random.Range(0, _enemyPrefabs.Length - 1);
                Instantiate(_enemyPrefabs[enemyPrefabNumber], _spawnPoints[spawnPointNumber].transform.position, Quaternion.identity);
            }
            _spawnAmount += _spawnAddAmount;
            yield return new WaitForSeconds(time);
        } while (spawnActive);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesPfs;
    [SerializeField] private int currentHordeIndex;
    [SerializeField] private Horde currentHorde;
    [SerializeField] private List<Transform> spawners;
    public static HordeSpawner main;
    private bool isSpawning;

    [System.Serializable]
    private struct Horde
    {
        public int numOfEnemies;
        public float spawnDelay;
    }

    [SerializeField] private List<Horde> enemyHordes;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        foreach (Transform spawner in transform)
        {
            spawners.Add(spawner);
        }

        currentHordeIndex = 0;
    }

    public IEnumerator SpawnHorde()
    {
        if (isSpawning) yield break;

        print("Starting horde: " + (currentHordeIndex + 1));

        isSpawning = true;
        currentHorde = enemyHordes[currentHordeIndex];
        Transform selectedSpawner = spawners[Random.Range(0, spawners.Count)];
        selectedSpawner.GetComponent<Spawner>().isActive = true;
        Vector2 hordeSpawnPoint = (Vector2)selectedSpawner.position + Vector2.down;

        for (int i = 0; i < currentHorde.numOfEnemies; i++)
        {
            Instantiate(enemiesPfs[0], hordeSpawnPoint, Quaternion.identity);
            yield return new WaitForSecondsRealtime(currentHorde.spawnDelay);
        }

        currentHordeIndex++;

        if (currentHordeIndex == enemyHordes.Count)
        {
            currentHordeIndex = 0;
        }

        isSpawning = false;
    }
}

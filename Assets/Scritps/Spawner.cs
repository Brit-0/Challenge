using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesPfs;
    [SerializeField] private int currentHordeIndex;
    [SerializeField] Horde currentHorde;
    private bool spawning;

    private struct Horde
    {
        public int numOfEnemies;
        public float spawnDelay;
    }

    [SerializeField] Horde[] enemyHordes = new Horde[3];

    private void Awake()
    {
        enemyHordes[0] = new Horde { numOfEnemies = 2, spawnDelay = 2 };
        enemyHordes[1] = new Horde { numOfEnemies = 4, spawnDelay = 1 };
        enemyHordes[2] = new Horde { numOfEnemies = 5, spawnDelay = .5f };
    }
    private void Start()
    {
        currentHordeIndex = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !spawning)
        {
            StartCoroutine("SpawnHorde");
        }
    }

    IEnumerator SpawnHorde()
    {
        spawning = true;
        currentHorde = enemyHordes[currentHordeIndex];

        for (int i = 0; i < currentHorde.numOfEnemies; i++)
        {
            Instantiate(enemiesPfs[0], transform.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(currentHorde.spawnDelay);
        }

        currentHordeIndex++;

        if (currentHordeIndex == enemyHordes.Length)
        {
            currentHordeIndex = 0;
        }

        spawning = false;
    }
}

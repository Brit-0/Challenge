using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class HordeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesPfs;
    [SerializeField] private int currentHordeIndex;
    [SerializeField] private Horde currentHorde;
    [SerializeField] private GameObject spawnPoint;
    public static HordeSpawner current;
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
        current = this;
        spawnPoint = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        currentHordeIndex = 0;
    }

    public IEnumerator SpawnHorde()
    {
        if (isSpawning) yield break;

        isSpawning = true;
        currentHorde = enemyHordes[currentHordeIndex];

        for (int i = 0; i < currentHorde.numOfEnemies; i++)
        {
            Instantiate(enemiesPfs[0], spawnPoint.transform.position, Quaternion.identity);
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

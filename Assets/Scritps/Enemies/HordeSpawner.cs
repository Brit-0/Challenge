using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeSpawner : MonoBehaviour
{
    public static HordeSpawner main;

    [SerializeField] private GameObject giantRat;
    [SerializeField] private GameObject smallRat;
    private Horde currentHorde;

    private float delayBetweenHordes = 10f;
    public List<Transform> activeSpawners = new();
    private bool isSpawning;

    [System.Serializable]
    private struct Horde
    {
        public int numOfEnemies;
        public List<GameObject> enemies;
        public float spawnDelay;
    }

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        foreach (Transform spawner in transform)
        {
            activeSpawners.Add(spawner);
        }

        GameManager.ChangeGamePhase += OpenSpawners;
    }
    
    private void OpenSpawners()
    {
        foreach (Transform spawner in activeSpawners)
        {
            spawner.GetComponent<Animator>().SetBool("isOpen", true);
            spawner.GetComponent<Spawner>().isOpen = true;
        }
    }

    public IEnumerator SpawnHorde()
    {
        if (isSpawning) yield break;

        isSpawning = true;
        currentHorde = CreateRandomHorde();
        Transform selectedSpawner = activeSpawners[Random.Range(0, activeSpawners.Count)];
        selectedSpawner.GetComponent<Spawner>().isActive = true;
        Vector2 hordeSpawnPoint = (Vector2)selectedSpawner.position + Vector2.down;

        for (int i = 0; i < currentHorde.numOfEnemies; i++)
        {
            Instantiate(currentHorde.enemies[i], hordeSpawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(currentHorde.spawnDelay);
        }

        isSpawning = false;

        yield return new WaitForSeconds(delayBetweenHordes);

        StartCoroutine(SpawnHorde());
    }

    private Horde CreateRandomHorde()
    {
        Horde newHorde = new() { spawnDelay = 2f, numOfEnemies = Random.Range(4, 15), enemies = new() { } };
        
        for (int i = 0; i < newHorde.numOfEnemies; i++)
        {
            if (Random.Range(1, 11) > 3)
            {
                newHorde.enemies.Add(smallRat);
            }
            else
            {
                newHorde.enemies.Add(giantRat);
            }
        }

        return newHorde;
    }

    public void CheckForWin()
    {
        if (activeSpawners.Count == 0)
        {
            GameManager.main.WinGame();
        }
    }
}

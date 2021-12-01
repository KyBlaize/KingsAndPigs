using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public bool faceRight;
    public string enemyName;
    public float spawnDelay;

    private float currentSpawnDelay;
    private GameController controller;
    private EnemyPool pool;
    [HideInInspector] public bool canSpawn = false;
    [HideInInspector] public bool gameOver = false; //replace with event

    private void Awake()
    {
        controller = FindObjectOfType<GameController>();
        pool = EnemyPool.instance;
        currentSpawnDelay = spawnDelay;
    }

    public void OnEnable()
    {
        controller.SpawnObject += Spawn;
        controller.EndPlay += ToggleSpawn;
    }

    public void OnDisable()
    {
        controller.SpawnObject -= Spawn;
        controller.EndPlay -= ToggleSpawn;
    }

    private void FixedUpdate()
    {
        if (canSpawn)
        {
            StartCoroutine(SpawnDelay(Random.Range(1, 2)));
        }
        else
        {
            if (!gameOver)
                currentSpawnDelay -= Time.deltaTime;
        }

        if (currentSpawnDelay <= 0)
        {
            canSpawn = true;
            currentSpawnDelay = 0;
        }
    }

    IEnumerator SpawnDelay(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < pool.enemies.Count; i++)
        {
            if (!pool.enemies[i].gameObject.activeSelf)
            {
                if (canSpawn)
                {
                    Spawn();
                    canSpawn = false;
                    currentSpawnDelay = spawnDelay;
                }
            }
        }
    }

    private void Spawn()
    {
        GameObject obj = pool.SpawnFromPool(enemyName, new Vector2(transform.position.x+(Random.Range(-2f,2f)), transform.position.y), Quaternion.identity);
        enemyController controller = obj.GetComponent<enemyController>();
        controller.right = faceRight;
    }

    private void ToggleSpawn() //stop spawning at game over
    {
        canSpawn = false;
    }
}

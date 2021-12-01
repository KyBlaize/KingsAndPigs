using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public List<enemySpawner> spawner = new List<enemySpawner>();
    public List<Door> doors = new List<Door>();
    public GameObject mainMenu, gameOver, gameScreen;
    public playerSpawner playerSpawn;

    public delegate void startGame();
    public event startGame SpawnObject;
    public delegate void endGame();
    public event endGame EndPlay;

    public Text text;
    private bool started = false;
    private float time, minutes, seconds;

    private void Awake()
    {
        var spawnerCount = FindObjectsOfType<enemySpawner>();
        for (int i = 0; i < spawnerCount.Length; i++)
        {
            spawner.Add(spawnerCount[i]);
        }
    }
    private void OnEnable()
    {
        var door = FindObjectsOfType<Door>();
        for (int i = 0; i < door.Length; i++)
        {
            doors.Add(door[i]);
            var subscribe = doors[i].GetComponent<Door>();
            subscribe.Touched += GameOver;
        }
    }
    private void FixedUpdate()
    {
        if (started)
        {
            time += Time.deltaTime;
        }
        minutes = Mathf.Floor(time / 60);
        seconds = Mathf.RoundToInt(time % 60);
        text.text = minutes + ":" + seconds;
    }

    private void OnDisable()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].Touched -= GameOver;
        }
    }

    public void GameOver()
    {
        gameScreen.SetActive(false);
        gameOver.SetActive(true);
        var enemies = FindObjectsOfType<enemyController>();
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].gameObject.activeSelf)
            {
                enemies[i].gameObject.BroadcastMessage("TakeDamage");
            }
        }
        var spawners = FindObjectsOfType<enemySpawner>();
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].canSpawn = false;
            spawners[i].gameOver = true;
        }
    }

    //---UI---
    public void StartGame()
    {
        mainMenu.SetActive(false);
        gameScreen.SetActive(true);
        started = true;
        SpawnObject?.Invoke();
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Challenge dealine too close
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Since There's only one level. Otherwise, return to main menu
    }
}

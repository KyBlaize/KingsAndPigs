using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSpawner : MonoBehaviour
{
    public GameObject player;

    private GameController controller;

    private void Awake()
    {
        controller = FindObjectOfType<GameController>();
    }

    public void OnEnable()
    {
        controller.SpawnObject += Spawn;
    }

    public void OnDisable()
    {
        controller.SpawnObject -= Spawn;
    }

    private void Spawn()
    {
        Instantiate(player);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour
{
    [SerializeField] private float maxTime;
    [SerializeField] private float heightRange;
    [SerializeField] private GameObject wall;
    [SerializeField] private float moveSpeed = 1.5f;

    private float timer;

    void Start()
    {
        SpawnWall();
    }

    void Update()
    {
        if (timer > maxTime)
        {
            SpawnWall();
            timer = 0;
        }

        timer += Time.deltaTime;

        MoveWalls();
    }

    void SpawnWall()
    {
        Vector3 spawnPos = transform.position + new Vector3(0, Random.Range(-heightRange, heightRange));
        GameObject _wall = Instantiate(wall, spawnPos, Quaternion.identity);

        Destroy(_wall, 10f);
    }

    void MoveWalls()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach (GameObject wall in walls)
        {
            wall.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
    }
}

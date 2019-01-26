using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies;
    public float spawnInterval = 32.0f;
    public int mobSize = 2;

    private float _timeToSpawn;
    private Vector3 screenLeft = new Vector3(-11.0f, 0.0f, -1.0f);
    private Vector3 screenRight = new Vector3(11.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        enemies = FindObjectsOfType<Enemy>().ToList();
        Spawn();
        _timeToSpawn = spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        _timeToSpawn -= Time.deltaTime;
        if (_timeToSpawn < 0)
        {
            Spawn();
            _timeToSpawn = spawnInterval;
        }
    }

    public void Spawn()
    {
        for(int i = 0; i < mobSize; i++)
        {
            if (i % 2 == 0) {         
                enemies.Add(Instantiate(enemies.FirstOrDefault(), screenLeft, new Quaternion()));
            } else
            {                
                enemies.Add(Instantiate(enemies.FirstOrDefault(), screenRight, new Quaternion()));
            }
        }
    }
}

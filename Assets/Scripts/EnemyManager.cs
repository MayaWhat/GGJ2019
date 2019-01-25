using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies;
    public float timeToSpawn;
    private Vector3 screenLeft = new Vector3(-11.0f, 0.0f, -1.0f);
    private Vector3 screenRight = new Vector3(11.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        enemies = FindObjectsOfType<Enemy>().ToList();
        Spawn(2);
        timeToSpawn = 32.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeToSpawn -= Time.deltaTime;
        if (timeToSpawn < 0)
        {
            Spawn(2);
            timeToSpawn = 32.0f;
        }
    }

    public void Spawn(int mobSize)
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Player _player;
    public List<Enemy> enemies;
    public MusicManager musicManager;
    private RoomManager _roomManager;
    public float spawnInterval = 10.0f;
    public int mobSize = 2;
    public float sqrMagnitudeForMusicTrigger;

    private float _timeToSpawn;
    private Vector3 screenLeft = new Vector3(-11.0f, 0.0f, -1.0f);
    private Vector3 screenRight = new Vector3(11.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        musicManager = FindObjectOfType<MusicManager>();
        enemies = FindObjectsOfType<Enemy>().ToList();
        _roomManager = FindObjectOfType<RoomManager>();
        //Spawn();
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

    private void FixedUpdate()
    {
        var smallestDistance = enemies.Min(x => (x.transform.position - _player.transform.position).sqrMagnitude);
        if(smallestDistance <= sqrMagnitudeForMusicTrigger)
        {
            var volume = 1 - (smallestDistance / sqrMagnitudeForMusicTrigger);
            musicManager.SetTrackVolume(TrackType.Bass, volume, 0.2f);
        }
    }

    public void Spawn()
    {
        for(int i = 0; i < mobSize; i++)
        {
            var pos = _roomManager.GetARandomRoomPosition();
            enemies.Add(Instantiate(enemies.FirstOrDefault(), pos, new Quaternion()));
            //if (i % 2 == 0) {         
            //    enemies.Add(Instantiate(enemies.FirstOrDefault(), screenLeft, new Quaternion()));
            //} else
            //{                
            //    enemies.Add(Instantiate(enemies.FirstOrDefault(), screenRight, new Quaternion()));
            //}
        }
    }
}

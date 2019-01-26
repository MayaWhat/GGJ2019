using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivePoint : MonoBehaviour
{
    private Player _player;
    private RoomManager _roomManager;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _roomManager = FindObjectOfType<RoomManager>();
        
        transform.position = GetRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.transform.position == transform.position)
        {
            Debug.Log("You got to point.");
            transform.position = GetRandomPosition();
        }
    }

    private Vector2 GetRandomPosition()
    {
        //var minX = Mathf.Max(1, _player.transform.position.x - 10);
        var randomX = Random.Range((int)_player.transform.position.x - 10, (int)_player.transform.position.x + 10);
        
        var minY = Mathf.Max(1, _player.transform.position.y - 15);
        var randomY = Random.Range((int)minY, (int)_player.transform.position.y + 10);

        var newPos = new Vector2(randomX, randomY);

        return newPos;
    }
}

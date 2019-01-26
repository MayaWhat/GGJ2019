using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _moveCd = 2f;
    private Vector3 _playerPosition;
    private float _timeToMove;
    private Player _player;
    private RoomManager _roomManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _timeToMove = _moveCd;
        Debug.Log(_playerPosition);
        _roomManager = FindObjectOfType<RoomManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _timeToMove -= Time.deltaTime;

        if (_timeToMove < 0)
        {
            FindPlayer();
            Vector3 move = CalculateMove();
            transform.Translate(move);
            _timeToMove = _moveCd;
        }
        //Debug.Log(this.transform.position);

    }

    private void FindPlayer()
    {                
        _playerPosition =  _player.transform.position;
    }

    private void GetBestExitPosition()
    {
        // TODO: Get all exits
        // TODO: Workout best exit
    }

    Vector3 CalculateMove()
    {
        Vector3 move;
        if (_playerPosition.y == transform.position.y)
        {
            if (_playerPosition.x > transform.position.x)
            {
                //move right
                move = new Vector3(1.0f, 0.0f);

            }
            else if (_playerPosition.x == transform.position.x)
            {
                
                //gotcha
                move = new Vector3(0f, 0f);
            }
            else
            {
                //move left
                move = new Vector3(-1.0f, 0.0f);
            }
        }
        else
        {
            //look for exit

            Room currentRoom = _roomManager.GetRoomAtPosition(transform.position);
            if(currentRoom == null) {
                //move into house
                if (transform.position.x < 0)
                {
                    transform.Translate(new Vector3(1.0f, 0.0f));
                }
                else {
                    transform.Translate(new Vector3(-1.0f, 0.0f));
                }
                
            }
            var stairs = _roomManager.IsStairAtPosition(transform.position, true);
            Debug.Log(stairs);
            if (stairs)
            {
                move = new Vector3(0f, 1f);
            } else
            {
                move = new Vector3(0.0f, 0.0f);
            }
        }
        return move;
    }
}

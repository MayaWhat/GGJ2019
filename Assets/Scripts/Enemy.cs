using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                return move = new Vector3(1.0f, 0.0f);

            }
            else if (_playerPosition.x == transform.position.x)
            {
                
                //gotcha
                return move = new Vector3(0f, 0f);
            }
            else
            {
                //move left
                return move = new Vector3(-1.0f, 0.0f);
            }
        }

        Room currentRoom = _roomManager.GetRoomAtPosition(transform.position);
        if(currentRoom == null && _playerPosition.y == transform.position.y) 
        {
            // if outside, go towards player
            return MoveX(_playerPosition);
        }
        else if (currentRoom == null)
        {
            // if outside, player on different y, we must be on the ground outside
            return MoveX(new Vector3(0, 0, 0));
        }

        // Find stairs
        bool lookForUpStairs = false;
        if (_playerPosition.y > transform.position.y) lookForUpStairs = true;
        var stairsToGoTo = currentRoom.stairs.FirstOrDefault(x => x.isUp == lookForUpStairs);
        if (stairsToGoTo != null)
        {
            if ((Vector2)transform.position == stairsToGoTo.position)
            {
                return MoveY(_playerPosition);
            }
            else
            {
                return MoveX(stairsToGoTo.position);
            }
        }
        
        return new Vector3();
    }
    
    Vector3 MoveX(Vector2 targetPos) 
    {
        if (transform.position.x < targetPos.x)
        {
            return new Vector3(1.0f, 0.0f);
        }
        else 
        {
            return new Vector3(-1.0f, 0.0f);
        }
    }

    Vector3 MoveY(Vector2 targetPos)
    {
        if (transform.position.y < targetPos.y)
        {
            return new Vector3(0f, 1.0f);
        }
        else 
        {
            return new Vector3(0, -1.0f);
        }
    }
}

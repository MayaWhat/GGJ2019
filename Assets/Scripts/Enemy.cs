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
    private Room currentRoom;
    private Vector2 _prevPosition;
    
    public List<Sprite> PossibleSprites;
    private SpriteRenderer _spriteRenderer;

    public bool CanMoveLeft;
    public bool CanMoveRight;
    public bool CanMoveUp;
    public bool CanMoveDown;
    public bool StairsUp;
    public bool StairsDown;
    public bool DoorRight;
    public bool DoorLeft;
    public string CurrentRoomName;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _spriteRenderer.sprite = PossibleSprites[Random.Range(0, PossibleSprites.Count)];

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
            currentRoom = _roomManager.GetRoomAtPosition(transform.position);
            
            FindAvailableMoves();
            Vector3 move;
            if ((_playerPosition - transform.position).magnitude > 8)
            {
                var validMoves = new List<Vector2>();
                if (CanMoveDown) validMoves.Add(new Vector2(0,-1));
                if (CanMoveUp) validMoves.Add(new Vector2(0,1));
                if (currentRoom == null || CanMoveLeft) validMoves.Add(new Vector2(-1,0));
                if (currentRoom == null || CanMoveRight) validMoves.Add(new Vector2(1,0));

                move = validMoves[Random.Range(0, validMoves.Count)];
            }
            else
            {
                move = CalculateMove();
            }
            if (!ValidateMove(move))
            {
                var validMoves = new List<Vector2>();
                if (CanMoveDown) validMoves.Add(new Vector2(0,-1));
                if (CanMoveUp) validMoves.Add(new Vector2(0,1));
                if (currentRoom == null || CanMoveLeft) validMoves.Add(new Vector2(-1,0));
                if (currentRoom == null || CanMoveRight) validMoves.Add(new Vector2(1,0));

                var newMove = validMoves[Random.Range(0, validMoves.Count)];

                _prevPosition = transform.position;
                transform.Translate(newMove);
            }
            else
            {
                _prevPosition = transform.position;
                transform.Translate(move);
            }
            _timeToMove = _moveCd;
        }
        //Debug.Log(this.transform.position);
        ResetBools();

        if ((Vector2)_playerPosition == (Vector2)transform.position)
        {
            _player.KillMe();
        }
    }

    private void FindPlayer()
    {                
        _playerPosition = _player.transform.position;
    }

    private void GetBestExitPosition()
    {
        // TODO: Get all exits
        // TODO: Workout best exit
    }

    bool ValidateMove(Vector2 move)
    {
        var newPos = (move + (Vector2)transform.position);
        var newRoom = _roomManager.GetRoomAtPosition(newPos);
        if (currentRoom == null && transform.position.y == 0)
        {
            // new pos isnt in a room and you arent on the ground
            return true;
        }

        if (move == new Vector2(1, 0) && CanMoveRight)
        {
            return true;
        }
        else if (move == new Vector2(-1, 0) && CanMoveLeft)
        {
            return true;
        }
        else if (move == new Vector2(0, 1) && CanMoveUp)
        {
            return true;
        }
        else if (move == new Vector2(0, -1) && CanMoveDown)
        {
            return true;
        }

        return false;
    }

    bool ValidateMoveBad(Vector2 move)
    {
        var newPos = (move + (Vector2)transform.position);
        var newRoom = _roomManager.GetRoomAtPosition(newPos);
        var myPos = (Vector2)transform.position;

        if (newRoom == null && transform.position.x != 0)
        {
            // new pos isnt in a room and you arent on the ground
            return false;
        }
        if (currentRoom != null && currentRoom.roomSpaces.Contains(newPos))
        {
            // In same room
            return true;
        }
        else if (newRoom != null) 
        {
            var currentRoomSpace = currentRoom.roomSpaces.First(x => x == myPos);
            var newRoomSpace = _roomManager.GetRoomAtPosition(newPos);
            // new pos is in a different room so check if we can actually get there
            if (move == new Vector2(1, 0) && currentRoom.doors.Any(x => x.isLeft && x.position == myPos)
                && newRoom.doors.Any(x => !x.isLeft && x.position == newPos))
            {
                return true;
            }
            else if (move == new Vector2(-1, 0) && currentRoom.doors.Any(x => !x.isLeft && x.position == myPos)
                && newRoom.doors.Any(x => x.isLeft && x.position == newPos))
            {
                return true;
            }
            else if (move == new Vector2(0, 1) && currentRoom.stairs.Any(x => x.isUp && x.position == myPos)
                && newRoom.stairs.Any(x => !x.isUp && x.position == newPos))
            {
                return true;
            }
            else if (move == new Vector2(0, -1) && currentRoom.stairs.Any(x => !x.isUp && x.position == myPos)
                && newRoom.stairs.Any(x => x.isUp && x.position == newPos))
            {
                return true;
            }
        }

        return false;
    }
    

    Vector3 CalculateMove()
    {
        Vector3 move;
        Room playersCurrentRoom = _roomManager.GetRoomAtPosition(_playerPosition);
        bool inSameRoomAsPlayer = false;
        if (currentRoom != null) inSameRoomAsPlayer = currentRoom.roomSpaces.Contains(_playerPosition);

        
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

        if (inSameRoomAsPlayer && _playerPosition.y == transform.position.y)
        {
            return MoveX(_playerPosition);
        }
        else if (inSameRoomAsPlayer && _playerPosition.y != transform.position.y)
        {
            var spaceToGoTo = currentRoom.roomSpaces
                .OrderBy(x => (x - (Vector2)transform.position).magnitude)
                .FirstOrDefault();
            if (spaceToGoTo != null)
            {
                if (transform.position.x == spaceToGoTo.x)
                {
                    return MoveY(_playerPosition);
                }
                else
                {
                    return MoveX(spaceToGoTo);
                }
            }
        }

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


        // Find stairs
        bool lookForUpStairs = false;
        if (_playerPosition.y > transform.position.y) lookForUpStairs = true;
        var stairsToGoTo = currentRoom.stairs.FirstOrDefault(x => x.isUp == lookForUpStairs);
        if (stairsToGoTo != null)
        {
            Vector2 result;
            if ((Vector2)transform.position == stairsToGoTo.position)
            {
                result = MoveY(_playerPosition);
            }
            else
            {
                result = MoveX(stairsToGoTo.position);
            }

            if (result == _prevPosition)
            {
                if (Random.Range(0, 1) == 0) return MoveX(_playerPosition);
                else return MoveX(_playerPosition);
            }
        }
        else
        {
            var spaceToGoTo = currentRoom.roomSpaces
                .OrderBy(x => (x - (Vector2)transform.position).magnitude)
                .FirstOrDefault();
            if (spaceToGoTo != null)
            {
                if (transform.position.x == spaceToGoTo.x)
                {
                    return MoveY(_playerPosition);
                }
                else
                {
                    return MoveX(spaceToGoTo);
                }
            }
        }
        
        return new Vector3();
    }

    private void FindAvailableMoves()
    {
        if (currentRoom == null) return;
        // Find left
        var leftPosition = (Vector2)transform.position + new Vector2(-1, 0);
        if (currentRoom.roomSpaces.Contains(leftPosition))
        {
            CanMoveLeft = true;
        }
        else if (currentRoom.doors.Any(x => x.position == (Vector2)transform.position && x.isLeft))
        {
            if (_roomManager.IsDoorAtPosition(leftPosition, doorIsLeft: false))
            {
                CanMoveLeft = true;
                DoorLeft = true;
            }
        }

        // Find right
        var rightPosition = (Vector2)transform.position + new Vector2(1, 0);
        if (currentRoom.roomSpaces.Contains(rightPosition))
        {
            CanMoveRight = true;
        }
        else if (currentRoom.doors.Any(x => x.position == (Vector2)transform.position && !x.isLeft))
        {
            if (_roomManager.IsDoorAtPosition(rightPosition, doorIsLeft: true))
            {
                CanMoveRight = true;
                DoorRight = true;
            }
        }

        // Find Up
        var upPosition = (Vector2)transform.position + new Vector2(0, 1);
        if (currentRoom.roomSpaces.Contains(upPosition))
        {
            CanMoveUp = true;
        }
        else if (currentRoom.stairs.Any(x => x.position == (Vector2)transform.position && x.isUp))
        {
            if (_roomManager.IsStairAtPosition(upPosition, stairIsUp: false))
            {
                CanMoveUp = true;
                StairsUp = true;
            }
        }

        // Find Bottom
        var downPosition = (Vector2)transform.position + new Vector2(0, -1);
        if (currentRoom.roomSpaces.Contains(downPosition))
        {
            CanMoveDown = true;
        }
        else if (currentRoom.stairs.Any(x => x.position == (Vector2)transform.position && !x.isUp))
        {
            if (_roomManager.IsStairAtPosition(downPosition, stairIsUp: true))
            {
                CanMoveDown = true;
                StairsDown = true;
            }
        }
    }
    
    Vector3 MoveX(Vector2 targetPos) 
    {
        if (targetPos == (Vector2)transform.position) return new Vector3(0,0);
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
        if (targetPos == (Vector2)transform.position) return new Vector3(0,0);
        if (transform.position.y < targetPos.y)
        {
            return new Vector3(0f, 1.0f);
        }
        else 
        {
            return new Vector3(0, -1.0f);
        }
    }
    
    void ResetBools()
    {
        CanMoveLeft = false;
        CanMoveDown = false;
        CanMoveRight = false;
        CanMoveUp = false;
        DoorLeft = false;
        DoorRight = false;
        StairsUp = false;
        StairsDown = false;
    }
}

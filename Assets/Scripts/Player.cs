using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public bool IsDead
    {
        get;
        private set;
    }
    private RoomManager _roomManager;
    private Room _currentRoom;
    public CinemachineVirtualCamera Cam;
    public MusicManager musicManager;
    private Animator _animator;

    public float rotSpeed;
    Quaternion startRot, endRot;

    private Vector3 _playerSpriteOriginalPosition;
    public Sprite NormalSprite;
    public Sprite DeathSprite;
    public Sprite BuildSprite;
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

    public int MoveFrames;
    public bool IsMoving;
    public bool Building;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _roomManager = FindObjectOfType<RoomManager>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _playerSpriteOriginalPosition = _spriteRenderer.transform.localPosition;
        musicManager = FindObjectOfType<MusicManager>();

        startRot = Quaternion.LookRotation(transform.forward);
        endRot = Quaternion.LookRotation(-transform.forward);
        UpdateCurrentRoom();
    }

    public void KillMe()
    {

        if (IsDead) return;
        Debug.Log("You Died!");
        musicManager.DoDead();
        IsDead = true;
        _animator.SetTrigger("Death");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            SceneManager.LoadScene("Start", LoadSceneMode.Single);
        }

        if (IsDead) 
        {
            Cam.m_Lens.FieldOfView += Time.deltaTime * 1f;
            return;
        }

        if (Building)
        {
            _animator.SetBool("Building", true);
        }
        else
        {
            _animator.SetBool("Building", false);
        }

        if(!IsMoving && !Building)
        {
            var xDir = Input.GetAxis("Horizontal");
            var yDir = Input.GetAxis("Vertical");

            int moveX = 0;
            int moveY = 0;

            if (xDir > 0) moveX = 1;
            else if (xDir < 0) moveX = -1;
            else if (yDir < 0) moveY = -1;
            else if (yDir > 0) moveY = 1;
            

            StartCoroutine(HandleMovement(moveX, moveY));
        }

        UpdateCurrentRoom();
    }

    private void UpdateCurrentRoom()
    {
        _currentRoom = _roomManager.GetRoomAtPosition(transform.position);
        CurrentRoomName = _currentRoom.Name;

        ResetBools();
        FindAvailableMoves();
    }

    IEnumerator HandleMovement(int moveX, int moveY)
    {
        if (IsMoving ||
            (moveX == 0 && moveY == 0) ||
            !IsMoveValid(moveX, moveY))
        {
            yield break;
        }

        IsMoving = true;
        if (moveX == 1) _spriteRenderer.transform.localScale = new Vector3(1, 1, 1);
        else if (moveX == -1) _spriteRenderer.transform.localScale = new Vector3(-1, 1, 1);
        _animator.SetBool("Walking", true);

        float moveXPerFrame = moveX / (float)MoveFrames;
        float moveYPerFrame = moveY / (float)MoveFrames;

        for (int i = 0; i < MoveFrames; i++)
        {
            _spriteRenderer.transform.localPosition += new Vector3
            (
                moveXPerFrame,
                moveYPerFrame
            );

            yield return new WaitForFixedUpdate();
        }

        IsMoving = false;
        _animator.SetBool("Walking", false);
        transform.position += new Vector3
        (
            moveX,
            moveY
        );

        _spriteRenderer.transform.localPosition = _playerSpriteOriginalPosition;

        UpdateCurrentRoom();
    }

    private void FindAvailableMoves()
    {
        // Find left
        var leftPosition = (Vector2)transform.position + new Vector2(-1, 0);
        if (_currentRoom.roomSpaces.Contains(leftPosition))
        {
            CanMoveLeft = true;
        }
        else if (_currentRoom.doors.Any(x => x.position == (Vector2)transform.position && x.isLeft))
        {
            if (_roomManager.IsDoorAtPosition(leftPosition, doorIsLeft: false))
            {
                CanMoveLeft = true;
                DoorLeft = true;
            }
        }

        // Find right
        var rightPosition = (Vector2)transform.position + new Vector2(1, 0);
        if (_currentRoom.roomSpaces.Contains(rightPosition))
        {
            CanMoveRight = true;
        }
        else if (_currentRoom.doors.Any(x => x.position == (Vector2)transform.position && !x.isLeft))
        {
            if (_roomManager.IsDoorAtPosition(rightPosition, doorIsLeft: true))
            {
                CanMoveRight = true;
                DoorRight = true;
            }
        }

        // Find Up
        var upPosition = (Vector2)transform.position + new Vector2(0, 1);
        if (_currentRoom.roomSpaces.Contains(upPosition))
        {
            CanMoveUp = true;
        }
        else if (_currentRoom.stairs.Any(x => x.position == (Vector2)transform.position && x.isUp))
        {
            if (_roomManager.IsStairAtPosition(upPosition, stairIsUp: false))
            {
                CanMoveUp = true;
                StairsUp = true;
            }
        }

        // Find Bottom
        var downPosition = (Vector2)transform.position + new Vector2(0, -1);
        if (_currentRoom.roomSpaces.Contains(downPosition))
        {
            CanMoveDown = true;
        }
        else if (_currentRoom.stairs.Any(x => x.position == (Vector2)transform.position && !x.isUp))
        {
            if (_roomManager.IsStairAtPosition(downPosition, stairIsUp: true))
            {
                CanMoveDown = true;
                StairsDown = true;
            }
        }
    }

    private bool IsMoveValid(int moveX, int moveY)
    {
        if (moveX == 1 && CanMoveRight) 
        {
            return true;
        }
        else if (moveX == -1 && CanMoveLeft)
        {
            return true;
        }
        else if (moveY == 1 && CanMoveUp)
        {
            return true;
        }
        else if (moveY == -1 && CanMoveDown)
        {
            return true;
        }

        return false;
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

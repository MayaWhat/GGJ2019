﻿using System.Collections;
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
	private Vector3 _spriteOriginalPosition;
	private SpriteRenderer _spriteRenderer;

	public int MoveFrames;

	public bool CanMoveLeft;
	public bool CanMoveRight;
	public bool CanMoveUp;
	public bool CanMoveDown;
	public bool StairsUp;
	public bool StairsDown;
	public bool DoorRight;
	public bool DoorLeft;
	public bool IsMoving = false;
	public string CurrentRoomName;

	private List<Vector2> _queuedMoves = new List<Vector2>();

	// Start is called before the first frame update
	private void Start()
	{
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		_spriteRenderer.sprite = PossibleSprites[Random.Range(0, PossibleSprites.Count)];
		_spriteOriginalPosition = _spriteRenderer.transform.localPosition;

		_player = FindObjectOfType<Player>();
		_timeToMove = _moveCd;
		Debug.Log(_playerPosition);
		_roomManager = FindObjectOfType<RoomManager>();
	}

	// Update is called once per frame
	private void Update()
	{
		if (((Vector2)_player.transform.position - (Vector2)transform.position).sqrMagnitude < 0.9f)
		{
			_player.KillMe();
		}
		_timeToMove -= Time.deltaTime;

		if (!IsMoving && _timeToMove < 0)
		{
			IsMoving = true;

			currentRoom = _roomManager.GetRoomAtPosition(transform.position);
			_playerPosition = GetPlayerPosition();

			var move = new Vector2();

			if (_queuedMoves.Count > 0)
			{
				move = _queuedMoves.Last() - (Vector2)transform.position;
				_queuedMoves.RemoveAt(_queuedMoves.Count - 1);
			}
			else
			{
				if (currentRoom == null)
				{
					if (transform.position.x > 0)
					{
						move = new Vector2(-1, 0);
					}
					else
					{
						move = new Vector2(1, 0);
					}
				}
				else
				{
					_queuedMoves = PathFinding(transform.position, _playerPosition, _roomManager.usedSpaces);
					if (_queuedMoves.Count > 0)
					{
						move = _queuedMoves.Last() - (Vector2)transform.position;
						_queuedMoves.RemoveAt(_queuedMoves.Count - 1);
					}					
				}
			}

			_prevPosition = transform.position;
			StartCoroutine(HandleMovement(move));

			_timeToMove = _moveCd;
			IsMoving = false;
		}
		//Debug.Log(this.transform.position);
		ResetBools();
	}

	private class Space
	{
		public Vector2 position;
		public Space bestParent;
		public float? score;

		public Space(Vector2 pos)
		{
			position = pos;
		}
	}

	public float GetScore(Vector2 start, Vector2 target, Vector2 current)
	{
		return (target - current).magnitude + (current - start).magnitude;
	}

	private List<Vector2> PathFinding(Vector2 start, Vector2 target, HashSet<Vector2> map)
	{
		var _openSet = new HashSet<Space>();
		var _closedSet = new HashSet<Space>();

		var _path = new List<Vector2>();

		var currentSpace = new Space(start);

		while (currentSpace.position != target && (_openSet.Any() || !_closedSet.Any()))
		{
			_closedSet.Add(currentSpace);
			_openSet.Remove(currentSpace);
			_openSet.UnionWith(GetNeighboursAndPopulate(currentSpace, start, target, map, _openSet, _closedSet));

			currentSpace = BestSpace(_openSet);
			
		}

		if (currentSpace.position == target)
		{
			var curr = currentSpace;
			_path.Add(curr.position);

			while (curr.position != start)
			{
				_path.Add(curr.bestParent.position);
				curr = curr.bestParent;
			}
		}

		return _path;
	}

	private Space BestSpace(HashSet<Space> spaces)
	{
		return spaces.OrderBy(x => x.score).First();
	}

	private HashSet<Space> GetNeighboursAndPopulate(Space space, Vector2 start, Vector2 target, HashSet<Vector2> map, HashSet<Space> openSet, HashSet<Space> closedSet)
	{
		var neighbours = new HashSet<Space>
		{
			new Space(space.position + new Vector2(-1, 0)),
			new Space(space.position + new Vector2(1, 0)),
			new Space(space.position + new Vector2(0, 1)),
			new Space(space.position + new Vector2(0, -1))
		};

		foreach (var neighbour in neighbours.ToList())
		{
			if(!map.Contains(neighbour.position))
			{
				neighbours.Remove(neighbour);
				continue;
			}

			if(closedSet.Any(x => x.position == neighbour.position))
			{
				neighbours.Remove(neighbour);
				continue;
			}

			if (openSet.Any(x => x.position == neighbour.position))
			{
				neighbours.Remove(neighbour);
				continue;
			}

			FindAvailableMoves(neighbour.position);
			if (!ValidateMove(space.position, neighbour.position - space.position))
			{
				neighbours.Remove(neighbour);
				continue;
			}

			if (neighbour.score == null)
			{
				neighbour.score = GetScore(start, target, neighbour.position);
			}

			if (neighbour.bestParent == null || space.score < neighbour.bestParent.score)
			{
				neighbour.bestParent = space;
			}
		}

		return neighbours;
	}

	private Vector2 GetPlayerPosition()
	{
		return _player.transform.position;
	}

	private bool ValidateMove(Vector2 start, Vector2 moveDirection)
	{
		var newPos = start + moveDirection;
		var newRoom = _roomManager.GetRoomAtPosition(newPos);
		if (currentRoom == null && transform.position.y == 0)
		{
			// new pos isnt in a room and you arent on the ground
			return true;
		}

		if (moveDirection == new Vector2(1, 0) && CanMoveRight)
		{
			return true;
		}
		else if (moveDirection == new Vector2(-1, 0) && CanMoveLeft)
		{
			return true;
		}
		else if (moveDirection == new Vector2(0, 1) && CanMoveUp)
		{
			return true;
		}
		else if (moveDirection == new Vector2(0, -1) && CanMoveDown)
		{
			return true;
		}

		return false;
	}

	private IEnumerator HandleMovement(Vector2 move)
	{
		float moveXPerFrame = move.x / MoveFrames;
		float moveYPerFrame = move.y / MoveFrames;

		if (move.x != 0f)
		{
			_spriteRenderer.transform.localScale = new Vector3(move.x > 0 ? -1 : 1, 1, 1);
		}

		for (int i = 0; i < MoveFrames; i++)
		{
			_spriteRenderer.transform.localPosition = new Vector3
			(
				_spriteRenderer.transform.localPosition.x + moveXPerFrame,
				move.y != 0 ?
					_spriteRenderer.transform.localPosition.y + moveYPerFrame :
					_spriteOriginalPosition.y + Random.Range(-0.01f, 0.01f)
			);

			yield return new WaitForFixedUpdate();
		}

		transform.position += new Vector3
		(
			move.x,
			move.y
		);

		_spriteRenderer.transform.localPosition = _spriteOriginalPosition;
	}

	private void FindAvailableMoves(Vector2 fromPosition)
	{
		if (currentRoom == null) return;
		// Find left
		var leftPosition = fromPosition + new Vector2(-1, 0);
		if (currentRoom.roomSpaces.Contains(leftPosition))
		{
			CanMoveLeft = true;
		}
		else if (currentRoom.doors.Any(x => x.position == fromPosition && x.isLeft))
		{
			if (_roomManager.IsDoorAtPosition(leftPosition, doorIsLeft: false))
			{
				CanMoveLeft = true;
				DoorLeft = true;
			}
		}

		// Find right
		var rightPosition = fromPosition + new Vector2(1, 0);
		if (currentRoom.roomSpaces.Contains(rightPosition))
		{
			CanMoveRight = true;
		}
		else if (currentRoom.doors.Any(x => x.position == fromPosition && !x.isLeft))
		{
			if (_roomManager.IsDoorAtPosition(rightPosition, doorIsLeft: true))
			{
				CanMoveRight = true;
				DoorRight = true;
			}
		}

		// Find Up
		var upPosition = fromPosition + new Vector2(0, 1);
		if (currentRoom.roomSpaces.Contains(upPosition))
		{
			CanMoveUp = true;
		}
		else if (currentRoom.stairs.Any(x => x.position == fromPosition && x.isUp))
		{
			if (_roomManager.IsStairAtPosition(upPosition, stairIsUp: false))
			{
				CanMoveUp = true;
				StairsUp = true;
			}
		}

		// Find Bottom
		var downPosition = fromPosition + new Vector2(0, -1);
		if (currentRoom.roomSpaces.Contains(downPosition))
		{
			CanMoveDown = true;
		}
		else if (currentRoom.stairs.Any(x => x.position == fromPosition && !x.isUp))
		{
			if (_roomManager.IsStairAtPosition(downPosition, stairIsUp: true))
			{
				CanMoveDown = true;
				StairsDown = true;
			}
		}
	}

	private Vector3 MoveX(Vector2 targetPos)
	{
		if (targetPos == (Vector2)transform.position) return new Vector3(0, 0);
		if (transform.position.x < targetPos.x)
		{
			return new Vector3(1.0f, 0.0f);
		}
		else
		{
			return new Vector3(-1.0f, 0.0f);
		}
	}

	private Vector3 MoveY(Vector2 targetPos)
	{
		if (targetPos == (Vector2)transform.position) return new Vector3(0, 0);
		if (transform.position.y < targetPos.y)
		{
			return new Vector3(0f, 1.0f);
		}
		else
		{
			return new Vector3(0, -1.0f);
		}
	}

	private void ResetBools()
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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class Room : MonoBehaviour
{
	public HashSet<Vector2> availableSpaces = new HashSet<Vector2>();
	public HashSet<Vector2> roomShape = new HashSet<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(3, 0) };
	public HashSet<Vector2> roomSpaces = new HashSet<Vector2>();

	public List<Door> doors = new List<Door>() { new Door(new Vector2(0,0), true) };
	public List<Stair> stairs = new List<Stair>() { new Stair(new Vector2(0, 0), true) };

	public RoomManager roomManager;

	public string Name;
	// Start is called before the first frame update
	private void Start()
	{
		roomManager = FindObjectOfType<RoomManager>();

		foreach(var space in roomShape)
		{
			roomSpaces.Add(space + (Vector2)transform.position);
		}

		foreach (var space in roomSpaces)
		{
			Debug.Log(Name + " - " + space);
			availableSpaces.UnionWith(GetUpwardsNeighbouringSpaces(space));
		}
		availableSpaces.ExceptWith(roomSpaces);

		roomManager.AddRoom(this);
	}


	// Update is called once per frame
	private void Update()
	{
	}

	public HashSet<Vector2> GetUpwardsNeighbouringSpaces (Vector2 space)
	{
		var neighbours = new HashSet<Vector2>();
		neighbours.Add(space + new Vector2(-1, 0));
		neighbours.Add(space + new Vector2(1, 0));
		neighbours.Add(space + new Vector2(0, 1));

		return neighbours;
	}
}
	
[Serializable]
public class Door
{
	public Door(Vector2 pos, bool left)
	{
		position = pos;
		isLeft = left;
	}

	public Vector2 position;
	public bool isLeft;
}

[Serializable]
public class Stair
{
	public Stair(Vector2 pos, bool up)
	{
		position = pos;
		isUp = up;
	}

	public Vector2 position;
	public bool isUp;
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	public HashSet<Vector2> availableSpaces = new HashSet<Vector2>();
	public List<Vector2> shape = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(3, 0) };

	public HashSet<Vector2> roomSpaces = new HashSet<Vector2>();	

	public List<Door> doors = new List<Door>() { new Door(new Vector2(0, 0), true) };
	public List<Stair> stairs = new List<Stair>() { new Stair(new Vector2(0, 0), true) };

	public RoomManager roomManager;
	public bool isBlueprint = false;

    public MeshRenderer[] meshes = new MeshRenderer[0];

	public string Name;

	public Room(List<Vector2> roomShape, List<Door> roomDoors, List<Stair> roomStairs, bool roomIsBlueprint)
	{
		shape = roomShape;
		doors = roomDoors;
		stairs = roomStairs;
		isBlueprint = roomIsBlueprint;
	}

	// Start is called before the first frame update
	private void Start()
	{
		availableSpaces = new HashSet<Vector2>();
		roomSpaces = new HashSet<Vector2>();

        meshes = GetComponentsInChildren<MeshRenderer>();
		

		roomManager = FindObjectOfType<RoomManager>();

		foreach (var door in doors)
		{
			door.position += (Vector2)transform.position;
		}
		foreach (var stair in stairs)
		{
			stair.position += (Vector2)transform.position;
		}
		foreach (var space in shape)
		{
			roomSpaces.Add(space + (Vector2)transform.position);
		}

		if (!isBlueprint)
		{
			foreach (var space in roomSpaces)
			{
				availableSpaces.UnionWith(GetNeighbouringSpaces(space));
			}
			availableSpaces.ExceptWith(roomSpaces);

			roomManager.AddRoom(this);
		}
	}

	// Update is called once per frame
	private void Update()
	{
	}

	public HashSet<Vector2> GetNeighbouringSpaces(Vector2 space)
	{
		var neighbours = new HashSet<Vector2>
		{
			space + new Vector2(-1, 0),
			space + new Vector2(1, 0),
			space + new Vector2(0, 1),
		};

		if (space.y > 0)
		{
			neighbours.Add(space + new Vector2(0, -1));
		}

		return neighbours;
	}

	public void Hidden(bool hide)
	{
        foreach (var mesh in meshes)
        {
            mesh.enabled = !hide;
        }
	}

    public void SetColor(Color color)
    {
        foreach (var mesh in meshes)
        {
            mesh.material.color = color;
        }
    }

	public HashSet<Vector2> GetRoomSpaces
	{
		get
		{
			var theSpaces = new HashSet<Vector2>();

			foreach(var space in shape)
			{
				theSpaces.Add(space + (Vector2)transform.position);
			}

			return theSpaces;
		}
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
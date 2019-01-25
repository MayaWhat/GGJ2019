using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class Room : MonoBehaviour
{
	public List<Vector2> availableSpaces;
	public List<Vector2> roomSpaces = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(3, 0) };


	public List<Door> doors = new List<Door>() { new Door(new Vector2(0,0), true) };
	public List<Stair> stairs = new List<Stair>() { new Stair(new Vector2(0, 0), true) };





	// Start is called before the first frame update
	void Start()
    {


		availableSpaces = roomSpaces.Select(x => x + (Vector2)gameObject.transform.position).ToList();
		foreach (var space in availableSpaces)
		{
			Debug.Log(space);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
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

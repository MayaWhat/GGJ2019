using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
	private List<Room> _rooms = new List<Room>();
	public HashSet<Vector2> availableSpaces = new HashSet<Vector2>();

	public GameObject availableSpaceVisualiser;

    // Start is called before the first frame update
    void Start()
    {
		_rooms = FindObjectsOfType<Room>().ToList();
		foreach(var room in _rooms)
		{
			availableSpaces.UnionWith(room.availableSpaces);
		}
		VisualiseAvailableSpaces();
	}

	public void AddRoom(Room room)
	{
		_rooms.Add(room);
		availableSpaces.UnionWith(room.availableSpaces);
		VisualiseAvailableSpaces();
	}

	public Room GetRoomAtPosition(Vector2 pos)
	{
		var room = _rooms.FirstOrDefault(x => x.roomSpaces.Contains(pos));

		return room;
	}

	public bool IsDoorAtPosition(Vector2 pos, bool doorIsLeft)
	{
		var room = GetRoomAtPosition(pos);

		var door = room?.doors.FirstOrDefault(x => x.position == pos && x.isLeft == doorIsLeft);

		return door != null;
	}

	public bool IsStairAtPosition(Vector2 pos, bool stairIsUp)
	{
		var room = GetRoomAtPosition(pos);

		var stair = room?.stairs.FirstOrDefault(x => x.position == pos && x.isUp == stairIsUp);

		return stair != null;
	}

	public void VisualiseAvailableSpaces()
	{
		foreach (var space in availableSpaces)
		{
			Instantiate(availableSpaceVisualiser, space - new Vector2(0f, 0f), transform.rotation);
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}

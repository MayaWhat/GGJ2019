using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
	private List<Room> _rooms;
    // Start is called before the first frame update
    void Start()
    {
		_rooms = FindObjectsOfType<Room>().ToList();
		Debug.Log(_rooms.Count());
    }

	public void AddRoom(Room room)
	{
		_rooms.Add(room);
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
		Debug.Log("Check pos " + pos);
		var stair = room?.stairs.FirstOrDefault(x => x.position == pos && x.isUp == stairIsUp);

		return stair != null;
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}

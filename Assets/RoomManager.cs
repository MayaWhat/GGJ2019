using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
	private List<Room> _rooms = new List<Room>();
	public HashSet<Vector2> availableSpaces = new HashSet<Vector2>();
	public HashSet<Vector2> usedSpaces = new HashSet<Vector2>();
	private List<GameObject> _spaceVisualisers = new List<GameObject>();

	public GameObject availableSpaceVisualiser;
	public bool showAvailableSpaces = false;

	// Start is called before the first frame update
	private void Start()
	{
		_rooms = FindObjectsOfType<Room>().ToList();

		foreach (var room in _rooms)
		{
			availableSpaces.UnionWith(room.availableSpaces);
		}
		VisualiseAvailableSpaces();
		
	}
	public bool CanPlaceRoom(Room room, Vector2 position)
	{
		var roomSpaces = room.GetRoomSpaces;

		if (!roomSpaces.Any(x => availableSpaces.Contains(x)))
		{
			// If none of the spaces are in available spaces
			return false;
		}

		if(roomSpaces.Any(x => usedSpaces.Contains(x)))
		{
			// If any of the spaces are already taken
			return false;
		}

		return true;
	}

	public void AddRoom(Room room)
	{
		_rooms.Add(room);
		availableSpaces.UnionWith(room.availableSpaces);
		usedSpaces.UnionWith(room.roomSpaces);
		availableSpaces.ExceptWith(usedSpaces);
		
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

	public Door GetDoorAtPosition(Vector2 pos)
	{
		var room = GetRoomAtPosition(pos);

		var door = room?.doors.FirstOrDefault(x => x.position == pos);

		return door;
	}

	public bool IsStairAtPosition(Vector2 pos, bool stairIsUp)
	{
		var room = GetRoomAtPosition(pos);
		var stair = room?.stairs.FirstOrDefault(x => x.position == pos && x.isUp == stairIsUp);

		return stair != null;
	}

	public void VisualiseAvailableSpaces()
	{
		if (!showAvailableSpaces)
		{
			return;
		}

		foreach (var visualiser in _spaceVisualisers)
		{
			Destroy(visualiser);
		}

		foreach (var space in availableSpaces)
		{
			_spaceVisualisers.Add(Instantiate(availableSpaceVisualiser, space, transform.rotation));
		}
	}

	// Update is called once per frame
	private void Update()
	{
	}
}
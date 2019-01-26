using System;
using UnityEngine;

public class Build : MonoBehaviour
{
	public Vector2 blueprintPosition;

	public Room roomBlueprint;
	public GameObject roomBlueprintObject;
	public GameObject roomObject;

	public bool blueprinting = true;
	
	public RoomManager roomManager;

	// Start is called before the first frame update
	private void Start()
	{
		roomManager = FindObjectOfType<RoomManager>();

		roomBlueprintObject = Instantiate(roomObject, transform);
		roomBlueprint = roomBlueprintObject.GetComponent<Room>();
		roomBlueprint.isBlueprint = true;

		roomBlueprint.Hidden(true);		
	}

	// Update is called once per frame
	private void Update()
	{
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        var position = mouseRay.GetPoint(10);
        blueprintPosition = SnapToGrid(position);

        if (blueprinting)
		{
			roomBlueprint.Hidden(false);
			roomBlueprint.transform.position = blueprintPosition;

			if (!roomManager.CanPlaceRoom(roomBlueprint, blueprintPosition))
			{
				roomBlueprint.SetColor(Color.red);
			}
			else
			{
				roomBlueprint.SetColor(Color.green);

				if (Input.GetMouseButtonDown(0))
				{
					BuildRoom();
				}
			}
		}
	}

	private Room MakeBlueprint(Room room)
	{
		return new Room(room.shape, room.doors, room.stairs, true);
	}

	private Vector2 SnapToGrid(Vector2 position)
	{
		return new Vector2((float)Math.Round(position.x, 0), (float)Math.Round(position.y, 0));
	}

	private void BuildRoom()
	{
		Instantiate(roomObject, blueprintPosition, transform.rotation);
	}
}
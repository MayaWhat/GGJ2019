using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Build : MonoBehaviour
{
	public Vector2 blueprintPosition;

	public Room roomBlueprint;
	public GameObject roomBlueprintObject;
	public List<GameObject> roomObjects;
	public int currentRoom = 0;

	public bool blueprinting = false;
	
	public RoomManager roomManager;

	// Start is called before the first frame update
	private void Start()
	{
		roomManager = FindObjectOfType<RoomManager>();

		roomBlueprintObject = Instantiate(roomObjects[currentRoom], transform);
		roomBlueprint = roomBlueprintObject.GetComponent<Room>();
		roomBlueprint.isBlueprint = true;

		roomBlueprint.Hidden(true);		
	}

	// Update is called once per frame
	private void Update()
	{
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        var position = mouseRay.GetPoint(10);
        blueprintPosition = SnapToGrid((Vector2)position - GetCentreOfShape(roomBlueprint.shape));

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
					if(currentRoom == roomObjects.Count - 1)
					{
						currentRoom = -1;
					}
					ChangeRoom(currentRoom + 1);
				}
			}
		}
	}

	public void ChangeRoom(int roomNumber)
	{
		if (roomNumber < 0 || roomNumber >= roomObjects.Count)
		{
			return;
		}

		currentRoom = roomNumber;
		Destroy(roomBlueprintObject);
		roomBlueprintObject = Instantiate(roomObjects[currentRoom], transform);
		roomBlueprint = roomBlueprintObject.GetComponent<Room>();
		roomBlueprint.isBlueprint = true;
		roomBlueprint.Hidden(true);
	}

	private Vector2 GetCentreOfShape(List<Vector2> Shape)
	{
		var xMax = Shape.OrderByDescending(x => x.x).First().x + 1;
		var yMax = Shape.OrderByDescending(x => x.y).First().y + 1;

		return new Vector2(xMax / 2, yMax / 2);
	}

	private Vector2 SnapToGrid(Vector2 position)
	{
		return new Vector2((float)Math.Round(position.x, 0), (float)Math.Round(position.y, 0));
	}

	private void BuildRoom()
	{
		Instantiate(roomObjects[currentRoom], blueprintPosition, transform.rotation);
	}
}
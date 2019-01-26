using System;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{
	public Room roomBlueprint;
    public Room currentRoom;

	public bool blueprinting = true;
	
	public RoomManager roomManager;
    public InventoryManager inventoryManager;

	// Start is called before the first frame update
	private void Start()
	{
		roomManager = FindObjectOfType<RoomManager>();
        inventoryManager = FindObjectOfType<InventoryManager>();

        //roomBlueprintObject = Instantiate(roomObjects[currentRoom], transform);
        //roomBlueprint = roomBlueprintObject.GetComponent<Room>();
        //roomBlueprint.isBlueprint = true;

        //roomBlueprint.Hidden(true);		
    }

	// Update is called once per frame
	private void Update()
	{
        if (blueprinting)
		{
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            var position = mouseRay.GetPoint(10);
            var blueprintPosition = SnapToGrid(position);

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

	public void BeginBuild(Room room)
	{
        inventoryManager.InventoryDisabled = true;
        currentRoom = room;
        if(roomBlueprint != null)
        {
            Destroy(roomBlueprint.gameObject);
        }
		
		roomBlueprint = Instantiate(currentRoom, transform);
		roomBlueprint.isBlueprint = true;
        blueprinting = true;
	}		

	private Vector2 SnapToGrid(Vector2 position)
	{
		return new Vector2((float)Math.Round(position.x, 0), (float)Math.Round(position.y, 0));
	}

	private void BuildRoom()
	{
        Instantiate(currentRoom, roomBlueprint.transform.position, transform.rotation);
        Destroy(roomBlueprint.gameObject);
        roomBlueprint = null;
        blueprinting = false;
        inventoryManager.InventoryDisabled = false;
    }
}
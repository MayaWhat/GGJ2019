using UnityEngine;

public class Build : MonoBehaviour
{
	public Vector3 buildDirection;
	public GameObject roomBlueprint;
	public GameObject room;
	public bool blueprinting = true;
	public GameObject currentRoom;
	public Rigidbody2D builder;
	public RoomManager roomManager;

	// Start is called before the first frame update
	private void Start()
	{
		roomManager = FindObjectOfType<RoomManager>();
		roomBlueprint.SetActive(false);
		builder = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	private void Update()
	{
		

		buildDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		buildDirection.z = 0;

		if (blueprinting)
		{
			roomBlueprint.SetActive(true);
			roomBlueprint.transform.position = buildDirection;
		}

		if (blueprinting && Input.GetMouseButtonDown(0))
		{
			PlaceBuilding();
		}
	}

	//public Vector2 BestAvailableSpace()
	//{
	//	var bestSpace = new Vector2();
	//	var gradient = (buildDirection.x - transform.position.x) / (buildDirection.y - transform.position.y);
	//	var line = 


	//	foreach (var availableSpace in roomManager.availableSpaces)
	//	{
	//		if (availableSpace)
	//	}

	//	return bestSpace;
	//}

	private void PlaceBuilding()
	{
		Instantiate(room, buildDirection, transform.rotation);
	}
}
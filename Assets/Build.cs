using UnityEngine;

public class Build : MonoBehaviour
{
	public Vector3 buildDirection;
	public GameObject roomBlueprint;
	public GameObject room;
	public bool blueprinting = true;
	public GameObject currentRoom;
	public Rigidbody2D builder;

	// Start is called before the first frame update
	private void Start()
	{
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

	private GameObject GetCurrentRoom()
	{
		return null;
	}

	private void PlaceBuilding()
	{
		Instantiate(room, buildDirection, transform.rotation);
	}
}
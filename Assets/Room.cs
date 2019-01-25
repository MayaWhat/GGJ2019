using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
	public List<Vector2> availableSpaces;
	public List<Vector2> roomSpaces = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(3, 0) };

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

	public List<Vector2> occupiedSpaces()
	{
		return new List<Vector2>();
	}
}

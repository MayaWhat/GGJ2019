using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectivePoint : MonoBehaviour
{
    private Player _player;
    private RoomManager _roomManager;
    private Score _score;
    public AudioSource yay;

    public List<Vector2> StaticPoints;
    private int _pointsGot = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _roomManager = FindObjectOfType<RoomManager>();
        _score = FindObjectOfType<Score>();
        
        if (StaticPoints.Any())
        {
            transform.position = StaticPoints[0];
        }
        else
        {
            transform.position = GetRandomPosition();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.transform.position == transform.position)
        {
            Debug.Log("You got to point.");
            yay.Play();
            _score.AddPoints(1);
            _pointsGot++;

            if (StaticPoints.Count > _pointsGot)
            {
                transform.position = StaticPoints[_pointsGot];
            }
            else
            {
                transform.position = GetRandomPosition();                
            }
        }
    }

    private Vector2 GetRandomPosition()
    {
        //var minX = Mathf.Max(1, _player.transform.position.x - 10);
        var randomX = Random.Range((int)_player.transform.position.x - 7, (int)_player.transform.position.x + 7);
        
        var minY = Mathf.Max(1, _player.transform.position.y - 10);
        var randomY = Random.Range((int)minY, (int)_player.transform.position.y + 10);

        var newPos = new Vector2(randomX, randomY);

        return newPos;
    }
}

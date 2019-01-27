using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreBoard;

    private int _score;
    // Start is called before the first frame update
    void Start()
    {
        _score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreBoard.text = _score.ToString();
    }

    public void AddPoints(int points)
    {
        _score += points;
    }
}
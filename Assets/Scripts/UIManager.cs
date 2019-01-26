using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<UIBuildOption> BuildOptions;
    public List<Room> Rooms;

    int _buildOptionSelected = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            _buildOptionSelected = 1;
        }
        else if (Input.GetKeyDown("2"))
        {
            _buildOptionSelected = 2;
        }
        else if (Input.GetKeyDown("3"))
        {
            _buildOptionSelected = 3;
        }
        else if (Input.GetKeyDown("4"))
        {
            _buildOptionSelected = 4;
        }
        else if (Input.GetKeyDown("5"))
        {
            _buildOptionSelected = 5;
        }
    }

    void FixedUpdate()
    {
        if (_buildOptionSelected > 0)
        {
            BuildOptions[_buildOptionSelected - 1].Build();
        }

        _buildOptionSelected = 0;
    }

    void UpdateUI()
    {
        
    }
}

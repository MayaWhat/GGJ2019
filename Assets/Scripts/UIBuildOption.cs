using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIBuildOption : MonoBehaviour
{
    private Image _image;
    public Room Room;

    private Build _builder;
    
    // Start is called before the first frame update
    void Start()
    {
        _builder = FindObjectOfType<Build>();
        _image = GetComponentInChildren<Image>();
        _image.sprite = Room.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Build()
    {
        Debug.Log("Start build mode");
        
    }
}

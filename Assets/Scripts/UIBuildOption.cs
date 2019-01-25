using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIBuildOption : MonoBehaviour
{
    private Image _image;
    
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponentInChildren<Image>();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public SpriteRenderer bg;
    public SpriteRenderer fog;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            
            SceneManager.LoadScene("ActualGame", LoadSceneMode.Single);
        }

        if(Input.GetButtonDown("Inventory Cancel"))
        {
            Application.Quit();
        }
    }
}

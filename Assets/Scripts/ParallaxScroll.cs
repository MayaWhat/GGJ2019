using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    public Transform LinkedTo;
    public float ScrollFactor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3
        (
            LinkedTo.position.x * ScrollFactor,
            LinkedTo.position.y * ScrollFactor,
            transform.position.z
        );
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public int maxStars = 1000;
    public int universeSize = 10;

    private ParticleSystem.Particle[] _points;
    public ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        _points = new ParticleSystem.Particle[maxStars];

        for(int i = 0; i < maxStars; i++)
        {
            _points[i].position = Random.insideUnitSphere * universeSize;
            _points[i].startSize = Random.Range(0.15f, 0.27f);
            _points[i].startColor = new Color(1, 1, 1, 1);
        }
        
        //particleSystem = gameObject.GetComponent<ParticleSystem>();
        particleSystem.SetParticles(_points, _points.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

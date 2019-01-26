using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSounds : MonoBehaviour
{
    public AudioClip[] Sounds;
    public AudioSource Player;
    public float MinInterval;
    public float MaxInterval;

    public float ElapsedTime = 0;
    public float CurrentInterval;

    public int LastSound = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.isPlaying)
        {
            return;
        }

        if(ElapsedTime == 0)
        {
            CurrentInterval = Random.Range(MinInterval, MaxInterval);
        }

        ElapsedTime += Time.deltaTime;

        if (ElapsedTime > CurrentInterval)
        {
            ElapsedTime = 0;
            var random = Random.Range(0, Sounds.Length);
            if (random == LastSound)
            {
                random = (random + 1) % Sounds.Length;
            }

            LastSound = random;

            Player.clip = Sounds[random];
            Player.Play();
        }
    }
}

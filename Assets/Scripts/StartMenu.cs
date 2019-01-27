using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public SpriteRenderer bg;
    public SpriteRenderer fog;
    public SpriteRenderer start;
    public SpriteRenderer title;
    public SpriteRenderer fade;

    public MusicManager music;

    public float ElapsedTime;
    public bool Changing = false;

    public Vector2 fogDirection = Vector2.right;
    public Vector2 fogMaxMove;
    public float fogMoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Jitter(start, 0.4f, 0.01f);
        Jitter(title, 0.6f, 0.01f);
        MoveFog();

        if (!Changing && Input.GetButtonDown("Start"))
        {
            Changing = true;
            StartCoroutine(FadeLevelChange());
        }

        if(Input.GetButtonDown("Inventory Cancel"))
        {
            Application.Quit();
        }
    }

    void MoveFog()
    {
        var newPosition = fog.transform.localPosition + (Vector3)(fogDirection * fogMoveSpeed);

        if (Mathf.Abs(newPosition.x) > fogMaxMove.x || Mathf.Abs(newPosition.y) > fogMaxMove.y)
        {
            fogDirection = new Vector2
            (
                Random.Range(-1f, 1f),
                0f
            );

            MoveFog();
            return;
        }

        fog.transform.localPosition = newPosition;
    }

    void Jitter(SpriteRenderer spriteRenderer, float chance, float range)
    {
        if (Random.Range(0f, 1f) < chance)
        {
            return;
        }

        spriteRenderer.transform.localPosition = new Vector3
        (
            Random.Range(-range, range),
            Random.Range(-range, range),
            spriteRenderer.transform.localPosition.z
        );
    }

    IEnumerator FadeLevelChange()
    {
        float timeToTake = 2f;

        music.SetTrackVolume(TrackType.Bass, 0, timeToTake);
        music.SetTrackVolume(TrackType.Drums, 0, timeToTake);
        float fadeAmount = 1f;

        while (fadeAmount > 0f)
        {
            fadeAmount -= (Time.deltaTime / timeToTake);
            fade.color = new Color(0f, 0f, 0f, 1f - fadeAmount);

            yield return null;
        }

        fade.color = new Color(0f, 0f, 0f, 1f);
        SceneManager.LoadScene("ActualGame", LoadSceneMode.Single);
    }
}

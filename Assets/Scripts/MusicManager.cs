using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSourceInfo[] TrackInfo;

    // Start is called before the first frame update
    void Start()
    {
        SetTrackVolume(TrackType.Base, 1f, 0f);
        SetTrackVolume(TrackType.Bass, 0f, 0f);
        SetTrackVolume(TrackType.Drums, 0f, 0f);

        foreach (var track in TrackInfo)
        {
            track.AudioSource.Play();
        }
    }

    public void SetTrackVolume(TrackType track, float volume, float time)
    {
        var info = TrackInfo.First(x => x.Track == track);
        if (info.Coroutine != null)
        {
            StopCoroutine(info.Coroutine);
        }

        info.Coroutine = StartCoroutine(LerpTrackVolume(info, volume, time));
    }

    private IEnumerator LerpTrackVolume(AudioSourceInfo audioSourceInfo, float volume, float time)
    {
        volume *= 0.6f;

        float timeElapsed = 0f;
        float startVolume = audioSourceInfo.AudioSource.volume;

        float volumeDiff = volume - startVolume;

        do
        {
            yield return null;
            timeElapsed += Time.deltaTime;

            audioSourceInfo.AudioSource.volume = startVolume + (volumeDiff / (time / timeElapsed));
        } while (timeElapsed < time);

        audioSourceInfo.AudioSource.volume = volume;

        audioSourceInfo.Coroutine = null;
    }

    public void DoDead()
    {
        StartCoroutine(LerpPitch(5f));
    }

    private IEnumerator LerpPitch(float time)
    {
        float timeElapsed = 0f;

        do
        {
            yield return null;
            timeElapsed += Time.deltaTime;

            foreach (var audioSourceInfo in TrackInfo)
            {
                audioSourceInfo.AudioSource.pitch = 1f - (timeElapsed / time);
            }


        } while (timeElapsed < time);

        foreach (var audioSourceInfo in TrackInfo)
        {
            audioSourceInfo.AudioSource.Stop();
        }
    }
}

public enum TrackType
{
    Base,
    Bass,
    Drums
}

[Serializable]
public class AudioSourceInfo
{
    public TrackType Track;
    public AudioSource AudioSource;
    public Coroutine Coroutine;
}
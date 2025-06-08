using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance => instance;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource fxSource;
    [SerializeField] float lowPitchRange = 0.95f;
    [SerializeField] float highPitchRange = 1.05f;    

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
            instance = this;

        DontDestroyOnLoad(gameObject);    
    }

    public void PlaySingle(AudioClip clip)
    {
        fxSource.clip = clip;
        fxSource.Play();
    }
    
    public void RandomizeFx(params AudioClip[] clips)
    {
        int n = Random.Range(0, clips.Length);

        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        fxSource.pitch = randomPitch;

        fxSource.clip = clips[n];

        fxSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

}

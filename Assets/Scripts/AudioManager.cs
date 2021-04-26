using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable()]
public struct SoundParameters
{
    [Range(0, 1)]
    public float Volume;
    [Range(-3, 3)]
    public float Pitch;
    public bool Loop;
}
[Serializable()]
public class Sound
{
    #region Variables

    [SerializeField] string name = string.Empty;
    public String Name { get { return name; } }

    [SerializeField] AudioClip clip = null;
    public AudioClip Clip { get { return clip; } }

    [SerializeField] SoundParameters parameters = new SoundParameters();
    public SoundParameters Parameters { get { return parameters; } }

    [HideInInspector]
    public AudioSource Source = null;
    #endregion

    public void Play()
    {
        Source.clip = Clip;

        Source.volume = Parameters.Volume;
        Source.pitch = Parameters.Pitch;
        Source.loop = Parameters.Loop;

        Source.Play();
    }
    public void Stop()
    {
        Source.Stop();
    }
}

public class AudioManager : MonoBehaviour
{
    #region Variables
    public static AudioManager Instance = null;

    [SerializeField] Sound[] sounds = null;
    [SerializeField] AudioSource sourcePrefab = null;

    [SerializeField] string startupTrack = string.Empty;

    #endregion

    #region Unity Defaults
    /// <summary>
    /// Function that is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    /// </summary>
    void Awake()
    {
        if (Instance != null)
        { Destroy(gameObject); }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        InitSounds();
    }
    /// <summary>
    /// Function that is called when the script instance is being loaded.
    /// </summary>
    /*void Start()
    {
        if (string.IsNullOrEmpty(startupTrack) != true)
        {
            PlaySound(startupTrack);
        }
    }*/
    #endregion

    #region Audio Function
    /// <summary>
    /// Intialize Sound
    /// </summary>
    void InitSounds()
    {
        foreach (var sound in sounds)
        {
            AudioSource source = Instantiate(sourcePrefab, gameObject.transform);
            source.name = sound.Name;

            sound.Source = source;
        }
    }
    /// <summary>
    /// Play Sound
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name)
    {
        var sound = GetSound(name);
        if (sound != null)
        {
            sound.Play();    
        }
        else
        {
            Debug.LogWarning("Sound by the name " + name + " is not found! Issues occured at AudioManager.PlaySound()");
        }
    }
    /// <summary>
    /// Stop Sound
    /// </summary>
    /// <param name="name"></param>
    public void StopSound(string name)
    {
        var sound = GetSound(name);
        if (sound != null)
        {
            sound.Stop();
        }
        else
        {
            Debug.LogWarning("Sound by the name " + name + " is not found! Issues occured at AudioManager.StopSound()");
        }
    }

    public void StopAll()
    {
        foreach (var sound in sounds)
        {
            sound.Stop();
        }
    }

    public void ToggleMusic(string name, bool paused)
    {
        var sound = GetSound(name);
        if (sound != null)
        {
            if (paused == false)
            {
                
                StartCoroutine(SoundPause(name, paused, 0.4f));
            }
            else
            {
                StartCoroutine(SoundPause(name, paused, 0));
            }
        }
        else
        {
            Debug.LogWarning("Sound by the name " + name + " is not found! Issues occured at AudioManager.StopSound()");
        }
        
    }

    IEnumerator SoundPause(string name, bool paused, float targetSound)
    {
        var sound = GetSound(name);
        if (paused && sound != null)
        {
            float time = 1f;
            while (time > targetSound)
            {
                time -= Time.deltaTime;
                sound.Source.volume = time;
                yield return 0;
            }
            
        }
        else if (!paused && sound != null)
        {
            float time = 0f;
            while (time < targetSound)
            {
                time += Time.deltaTime;
                sound.Source.volume = time;
                yield return 0;
            }
        }
    }
    #endregion

    #region Getters
    Sound GetSound(string name)
    {
        foreach (var sound in sounds)
        {
            if (sound.Name == name)
            {
                return sound;
            }
        }
        return null;
    }

    #endregion
}

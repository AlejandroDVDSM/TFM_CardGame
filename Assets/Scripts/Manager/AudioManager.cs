using System;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] m_sounds;
    
    public static AudioManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Add an AudioSource and set it up for each sound
        foreach (Sound sound in m_sounds.Where(s => s.PlayOnAwake))
        {
            Play(sound.Name);
        }
    }

    /// <summary>
    /// Play an audio
    /// </summary>
    /// <param name="name">Name of the audio to be played</param>
    public void Play(string name)
    {
        Sound sound = Array.Find(m_sounds, s => s.Name.Equals(name));

        if (sound == null)
        {
            Debug.LogWarning($"[AudioManager] No sound found with name '{name}'");
            return;
        }

        if (!sound.Source)
        {
            CreateAudioSource(sound);
        }
            
        sound.Source.Play();
    }

    /// <summary>
    /// Set volume to 0 or 1 according to a bool
    /// </summary>
    /// <param name="mute">Dictates if the volume should be silenced or not</param>
    public void MuteAudio(bool mute)
    {
        AudioListener.volume = mute ? 0 : 1;
    }

    /// <summary>
    /// Check if the volume is 0
    /// </summary>
    /// <returns>True if the audio is muted. Otherwise, return false</returns>
    public bool IsAudioMuted()
    {
        return AudioListener.volume == 0;
    }

    /// <summary>
    /// Add an AudioSource component to the AudioManager game object with the specified sound
    /// </summary>
    /// <param name="sound">The sound that should be linked to the new audio source</param>
    private void CreateAudioSource(Sound sound)
    {
        sound.Source = gameObject.AddComponent<AudioSource>();
        sound.Source.clip = sound.Clip;
            
        sound.Source.volume = sound.Volume;
        sound.Source.pitch = sound.Pitch;
        sound.Source.loop = sound.Loop;
    }
}

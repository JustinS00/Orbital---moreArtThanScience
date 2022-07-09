using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup soundMixer;
    public Sound[] music;
    public Sound[] sounds;
    

    [Header("Music Settings")]
    [Range(0f, 1f)]
    public float musicVolume;
    public bool onMusic = true;
    
    [Header("Sound Settings")]
    [Range(0f, 1f)]
    public float soundVolume;
    public bool onSound = true;

    private float time = 0f;

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            Debug.Log("Destroying AudioManager");
            return;
        } else {
            instance = this;
        }
        foreach (Sound sound in music) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = onMusic ? musicVolume : 0f;
            sound.source.pitch = sound.pitch;
            sound.source.outputAudioMixerGroup = musicMixer;
        }
        foreach(Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = onSound ? soundVolume : 0f;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = soundMixer;
        }
    }

    public void UpdateVolume() {
        foreach (Sound sound in music) {
            sound.source.volume = onMusic ? musicVolume : 0f;
        }
        foreach(Sound sound in sounds) {
            sound.source.volume = onSound ? soundVolume : 0f;
        }      
    }


    private void PlayMusic() {
        Debug.Log("Play Music");
        int randIndex = UnityEngine.Random.Range(0, music.Length);
        Sound sound = music[randIndex];
        time = sound.source.clip.length;
        PlayMusic(sound.name);

    }

    private void Update() {
        if (time <= 0) {
            PlayMusic();
        }
        time -= Time.deltaTime;
    }

    public void PlayMusic(string name) {
        Play(name, music);
    }

    public void PlaySound(string name) {
        Play(name, sounds);
    }

    private void Play(string name, Sound[] array) {
        Sound sound = Array.Find(array, sound => sound.name == name);
        if (sound != null) {
            sound.source.Play();
            Debug.Log("Currently playing: " + name);
        } else {
            Debug.LogWarning("Audio clip: " + name + " not found!");
        }
    }
}

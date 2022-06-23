using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    public Sound[] sounds;

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            Debug.Log("Destroying AudioManager");
            return;
        } else {
            instance = this;
        }

        foreach(Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    void Start () {
        Play("Theme");
    }

    public void Play(string name) {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound != null) {
            sound.source.Play();
        } else {
            Debug.LogWarning("Audio clip: " + name + " not found!");
        }
    }
}

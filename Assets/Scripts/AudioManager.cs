using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    private Sound[][] _allSounds;
    [HideInInspector] public Sound currentSound;

    //Scenes
    Scene _currentScene;
    private string _sceneName;

    //Array of all Sounds
    public Sound[] music;
    public Sound[] effectSounds;

    private void Awake()
    {
        if(instance != null) Destroy(gameObject);
        instance = this;
    }

    private void OnEnable()
    {
        _allSounds = new[] { music, effectSounds };

        _currentScene = SceneManager.GetActiveScene();
        _sceneName = _currentScene.name;

        foreach (Sound[] sArr in _allSounds)
        {
            foreach (Sound s in sArr)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.loop = s.loop;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
            }
        }
    }

    private void Start()
    {
        if (_sceneName == "Menu" || _sceneName == "GameScene")
        {
            Play("Theme", music);
        }
    }

    public void Stop(string n, Sound[] soundArr)
    {
        //Plays the sound of the object with the name "name"
        for (int i = 0; i < soundArr.Length; i++)
        {
            if (soundArr[i].name == n)
            {
                currentSound = soundArr[i];
            }
        }
        currentSound.source.Stop();
    }
    
    public void Play(string n, Sound[] soundArr)
    {
        //Plays the sound of the object with the name "name"
        for (int i = 0; i < soundArr.Length; i++)
        {
            if (soundArr[i].name == n)
            {
                currentSound = soundArr[i];
            }
        }
        if(currentSound != null)
            currentSound.source.Play();
    }
}


[Serializable]
public class Sound
{
    public string name;
    public bool loop = false;
    [HideInInspector] public bool isPlaying = false;

    public AudioClip clip;

    [Range(0f, 1f)] public float volume;
    [Range(.1f, 3f)] public float pitch;

    [HideInInspector] public AudioSource source;
}
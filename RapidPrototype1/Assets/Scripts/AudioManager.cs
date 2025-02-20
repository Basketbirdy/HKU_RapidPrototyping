using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private SoundGroup[] soundGroups;

    private Dictionary<string, SoundGroup> sGroups = new Dictionary<string, SoundGroup>();

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }

        CreateSounds();

    }

    private void Start()
    {

    }

    private void CreateSounds()
    {
        // for each soundgroup
        for(int i = 0; i < soundGroups.Length; i++)
        {
            SoundGroup currentSoundGroup = soundGroups[i];
            currentSoundGroup.soundLookUp = new Dictionary<string, Sound>();

            for (int j = 0; j < soundGroups[i].sounds.Length; j++)
            {
                Sound currentSound = currentSoundGroup.sounds[j];

                // create new source
                AudioSource source = null;
                if (soundGroups[i].sourceObject == null)
                {
                    source = transform.gameObject.AddComponent<AudioSource>();
                }
                else
                {
                    source = soundGroups[i].sourceObject.transform.gameObject.AddComponent<AudioSource>();
                }

                SetupSource(source, currentSoundGroup, currentSound);
            }

            sGroups.Add(currentSoundGroup.identifier, currentSoundGroup);
        }
    }

    public void Play(string groupName, string soundName = null)
    {
        Sound soundToPlay = FindSound(groupName, soundName);
        Debug.Log($"Sound to play: {soundToPlay.identifier} & {string.Empty}");

        if(soundToPlay.identifier == null) { Debug.Log("Returning"); return; }

        soundToPlay.audioSource.Play();
        Debug.Log($"Played sound '{soundName}' from '{groupName}' group");
    }

    private Sound FindSound(string groupName, string soundName = null)
    {
        if (!sGroups.ContainsKey(groupName))
        {
            Debug.LogWarning($"Could not find sound group with name: {groupName}");
            return new Sound();
        }
        SoundGroup sg = sGroups[groupName];

        Sound soundToPlay = new Sound();
        if (soundName == null || !sg.soundLookUp.ContainsKey(soundName))
        {
            // get random sound from group
            int randIndex = Random.Range(0, sg.sounds.Length);
            soundToPlay = sg.soundLookUp[sg.sounds[randIndex].identifier];
        }
        else
        {
            // get specified sound
            soundToPlay = sg.soundLookUp[soundName];
        }

        return soundToPlay;
    }

    private void SetupSource(AudioSource source, SoundGroup soundGroup, Sound sound)
    {
        source.clip = sound.clip;

        source.volume = sound.volume;
        source.pitch = sound.pitch;

        source.loop = sound.loop;

        source.spatialBlend = sound.is3D ? 1 : 0; // if true x = 1, if false x = 0
        source.rolloffMode = sound.rolloffMode;
        source.minDistance = sound.minMaxRollof.x;
        source.maxDistance = sound.minMaxRollof.y;

        source.playOnAwake = false;

        sound.SetSource(source);

        soundGroup.soundLookUp.Add(sound.identifier, sound);

        foreach(KeyValuePair<string, Sound> s in soundGroup.soundLookUp)
        {
            Debug.Log($"Sound: {s.Value.identifier}");
        }
    }
}

[System.Serializable]
struct SoundGroup
{
    [Header("identification")]
    public string identifier;
    [Header("Settings")]
    public AudioMixerGroup audioGroup;
    public Transform sourceObject;
    [Space]
    public Sound[] sounds;
    [HideInInspector] public Dictionary<string, Sound> soundLookUp;
}

[System.Serializable]
struct Sound
{
    public Sound(bool isNull = true)
    {
        identifier = string.Empty;
        clip = null;
        volume = 0;
        pitch = 0;
        loop = false;
        is3D = false;
        rolloffMode = 0;
        minMaxRollof = Vector2.zero;
        audioSource = null;
        mute = false;
    }

    [Header("identification")]
    public string identifier;
    [Space]
    public AudioClip clip;
    [Header("Settings")]
    public float volume;
    public float pitch;
    public bool loop;
    [Space]
    public bool is3D;
    public AudioRolloffMode rolloffMode;
    public Vector2 minMaxRollof;

    // runtime
    public AudioSource audioSource;
    private bool mute;

    public void SetSource(AudioSource source)
    {
        audioSource = source;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class Audio : MonoBehaviour
{
    [Serializable]
    public class sfxLib
    {
        public string               name;
        [EventRef] public string    sfx_path;
    }

    public static Audio         Instance;

    [Header("Volume Sliders")]
    public float               master_volume        = 0.5f;

    //FMOD Variables
    private Bus                 master_bus;

    [Header("Events Selector")]
    [Space(20)]
    [HideInInspector]
    private string              sfx_dir              = "event:/";
    public List<sfxLib>         sfx_objects_list;

    [Space(20)]

    [HideInInspector]
    public bool                 audio_loaded        = false;

    // Sound accessibility
    public float                step_timer          = 0.3f;

    #region Setup
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    private void Start()
    {
        initAudio();
        master_bus = RuntimeManager.GetBus("Bus:/");
    }

    public void initAudio()
    {
        if (!audio_loaded)
        {
            FMOD.RESULT result =
                RuntimeManager.CoreSystem.mixerSuspend();
            result =
                RuntimeManager.CoreSystem.mixerResume();

            audio_loaded = true;
        }
    }
    #endregion

    #region Update volume
    private void Update()
    {
        master_bus.setVolume(master_volume);
    }

    public void UpdateVolume(float mv)
    {
        master_volume = mv;
    }
    #endregion

    #region Play flat sound
    public void Play2DSound(string event_name)
    {
        foreach (sfxLib sfx in sfx_objects_list)
        {
            if (event_name == sfx.name)
                PlayOneShot(sfx.sfx_path, master_volume);
        }

        if (!sfx_objects_list.Exists(x => x.name == event_name))
            Debug.LogWarning("The event name " + event_name + " does not match any of the sounds in the Audio Manager's SFX Objects List, please check the Audio Manager's objects list");
    }
    
    private static void PlayOneShot(string path, float volume)
    {
        try
        {
            var instance =
            RuntimeManager.CreateInstance(path);

            instance.setVolume(volume);
            instance.start();
            instance.release();
        }

        catch (EventNotFoundException)
        {
            Debug.LogWarning("[FMOD] Event not found: " + path);
        }
    }
    #endregion

    #region Play directional sound + Footsteps
    private float _timer = 0.0f;
    public void PlayFootstep(GameObject player, float velocity, ref float timer)
    {
        if (velocity > 1.0f)
        {
            timer += Time.deltaTime;
            if (timer >= step_timer)
            {
                PlayOneShotAttached(
                    sfx_dir + "Walk",
                    master_volume,
                    player
                    );
                timer = 0.0f;
            }
        }
        else
        {
            _timer += Time.deltaTime;
            if (_timer >= 0.5f)
            {
                timer = step_timer;
                _timer = 0.0f;
            }
        }
    }

    // Play sound at object
    public void Play3DAway(string event_name, GameObject obj)
    {
        GameObject sound =
            new GameObject(event_name);

        sound.transform.position =
            obj.transform.position;

        sound.AddComponent<AudioPlaySoundHere>();
    }

    // Play sound at player
    public void Play3DLocal(string event_name, GameObject obj)
    {
        foreach (sfxLib sfx in sfx_objects_list)
        {
            if (event_name == sfx.name)
                PlayOneShotAttached(sfx.sfx_path, master_volume, obj);
        }
        if (!sfx_objects_list.Exists(x => x.name == event_name))
            Debug.LogWarning("The event name " + event_name + " does not match any of the sounds in the Audio Manager's SFX Objects List, please check the Audio Manager's objects list");
    }

    private static void PlayOneShotAttached(string path, float volume, GameObject gameObject)
    {
        try
        {
            var instance =
                RuntimeManager.CreateInstance(path);

            instance.setVolume(volume);

            RuntimeManager.AttachInstanceToGameObject(
                instance,
                gameObject.transform,
                gameObject.GetComponent<Rigidbody>());

            instance.start();
            instance.release();
        }

        catch (EventNotFoundException)
        {
            Debug.LogWarning("[FMOD] Event not found: " + path);
        }
    }
    #endregion
}

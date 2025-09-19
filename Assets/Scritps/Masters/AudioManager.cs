using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager main;

    //private static AudioSource source;
    [SerializeField] private AudioSource musicSource;

    [Header("SOUND EFFECTS")]
    public AudioClip click;
    public AudioClip shoot;
    public AudioClip[] footsteps;
    public AudioClip lockpick;
    public AudioClip impact;
    public AudioClip chestOpen;
    public AudioClip doorOpen;
    public AudioClip doorClose;
    public AudioClip gate;
    public AudioClip lever;
    //RAT
    public AudioClip giantRatAttack;
    public AudioClip giantRatIdle;

    [Header("AMBIENCE")]
    public AudioClip dungeon;

    [Header("SOUNDTRACK")]
    public AudioClip music1;

    private void Awake()
    {
        main = this;
        //source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, float volume = .5f)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.Play();

        Destroy(source, clip.length);
    }

    public void PlayerSpatialSound(AudioClip clip, GameObject gameObj, float volume = .5f,float minDistance = 1, float maxDistance = 5000, AudioRolloffMode rollofMode = AudioRolloffMode.Logarithmic)
    {
        AudioSource source = gameObj.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 1f;
        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
        source.rolloffMode = rollofMode;
        source.Play();

        Destroy(source, clip.length);
    }
}

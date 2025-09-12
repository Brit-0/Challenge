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

    [Header("AMBIENCE")]
    public AudioClip dungeon;

    [Header("SOUNDTRACK")]
    public AudioClip music1;

    private void Awake()
    {
        main = this;
        //source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, float volume = .5f, bool overlap = false)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        if (overlap)
        {
            source.PlayOneShot(clip, volume);
        }

        source.clip = clip;
        source.volume = volume;
        source.Play();

        Destroy(source, clip.length);
    }
}

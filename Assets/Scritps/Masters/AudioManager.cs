using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager main;

    public AudioSource musicSource;

    [Header("SOUND EFFECTS")]
    public AudioClip shoot;
    public AudioClip[] footsteps;
    public AudioClip[] hurt;
    public AudioClip hit;
    public AudioClip lockpick;
    public AudioClip impact;
    public AudioClip chestOpen;
    public AudioClip doorOpen;
    public AudioClip doorClose;
    public AudioClip gate;
    public AudioClip lever;
    public AudioClip fleshImpact;
    public AudioClip torchIgnite;
    public AudioClip barricade;
    public AudioClip darkTransition;
    public AudioClip gameOver;
    public AudioClip searching;
    public AudioClip altarHit;
    public AudioClip stonePlace;
    public AudioClip bandage;

    [Header("ENEMIES")]
    public AudioClip giantRatAttack;
    public AudioClip giantRatIdle;
    public AudioClip slimeAttack;
    public AudioClip[] squeaks;
    public AudioClip slimeExplosion;

    [Header("AMBIENCE")]
    public AudioClip dungeon;

    [Header("SOUNDTRACK")]
    public AudioClip rockSoundtrack;

    [Header("UI")]
    public AudioClip uIImpact;
    public AudioClip uIBoom;
    public AudioClip select;
    public AudioClip click;

    private void Awake()
    {
        main = this;
    }

    public void PlaySound(AudioClip clip, float volume = .5f)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.Play();

        Destroy(source, clip.length);
    }

    public void PlaySpatialSound(AudioClip clip, GameObject gameObj, float volume = .5f,float minDistance = 1, float maxDistance = 5000, AudioRolloffMode rollofMode = AudioRolloffMode.Logarithmic)
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

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        musicSource.clip = clip;
        musicSource.volume = volume;  
        musicSource.Play();
    }

    public void PlaySoundOneShot(AudioClip clip, float volume = .5f)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        source.PlayOneShot(clip, volume);

        Destroy(source, clip.length + .1f);
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}

using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public enum GamePhase
{
    None,
    Exploration,
    Defense
}

public enum GameState
{
    Menu,
    Playing,
    Paused
}

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    public static GamePhase currentGamePhase = GamePhase.Exploration;
    public static GameState currentGameState = GameState.Playing;

    public static Action ChangeGamePhase;

    [Header("UI REFERENCES")]
    [SerializeField] private Image here;
    [SerializeField] private Image they;
    [SerializeField] private Image are;
    [SerializeField] private Canvas effectsCanvas;
    [SerializeField] private Image shadow;
    [SerializeField] private Image blackout;

    [Header("PREFABS")]
    [SerializeField] private GameObject breakParticle;

    [Header("REFERENCES")]
    [SerializeField] private Transform torches;
    [SerializeField] private Material fogMAT;
    [SerializeField] private ParticleSystem dustPs;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        fogMAT.SetColor("_FogColor", new Color(0, 0, .8f, .3f));
    }

    [ContextMenu("START DEFENSE")]
    private void CallStart()
    {
        StartCoroutine(StartDefensePhase());
    }

    public IEnumerator StartDefensePhase()
    {
        currentGamePhase = GamePhase.Defense;
        ChangeGamePhase.Invoke();

        /*here.rectTransform.position += Vector3.up * 50;
        they.rectTransform.position += Vector3.up * 50;
        are.rectTransform.position += Vector3.up * 50;*/

        effectsCanvas.gameObject.SetActive(true);
        shadow.DOFade(1f, 4f);

        yield return new WaitForSeconds(.8f);
        here.rectTransform.DOAnchorPos(new Vector3(0, 242, 0), .2f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.08f);
        HitFeedback(1);
        yield return new WaitForSeconds(.8f);
        they.rectTransform.DOAnchorPos(new Vector3(88.50001f, -42.19996f, 0), .2f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.08f);
        HitFeedback(2);
        yield return new WaitForSeconds(.8f);
        are.rectTransform.DOAnchorPos(new Vector3(-6.19999f, -183.5999f, 0), .2f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.08f);
        HitFeedback(3);

        yield return new WaitForSeconds(1f);

        IgniteTorches();
        fogMAT.SetColor("_FogColor", new Color(1, 0, 0, .3f));
        AudioManager.main.PlayMusic(AudioManager.main.rockSoundtrack, .1f);
        StartCoroutine(HordeSpawner.main.SpawnHorde());
        effectsCanvas.GetComponent<CanvasGroup>().DOFade(0, 3f);

        yield return new WaitForSeconds(3f);

        effectsCanvas.enabled = false;
    }

    private void IgniteTorches()
    {
        AudioManager.main.PlaySound(AudioManager.main.torchIgnite, 1f);

        foreach (Transform torch in torches)
        {
            torch.GetComponent<Animator>().SetBool("isOn", true);
        }
    }

    private void HitFeedback(int number)
    {
        Transform part;

        switch (number)
        {
            default:
            case 1: part = here.transform; break;
            case 2: part = they.transform; break;
            case 3: part = are.transform; break;
        }

        part.GetChild(0).GetComponent<ParticleSystem>().Play();
        CameraController.main.CameraShake(number, .3f);

        if (number != 3)
        {
            AudioManager.main.PlaySound(AudioManager.main.uIImpact, 1f);
        }
        else
        {
            AudioManager.main.PlaySound(AudioManager.main.uIBoom, 1f);
        }
    }

    public void WinGame()
    {
        blackout.DOFade(1f, 3f).OnComplete(() => { SceneManager.LoadScene("Final Screen"); });
        StartCoroutine(AudioManager.FadeOut(AudioManager.main.musicSource, 3f));
    }
}

using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using DG.Tweening;

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

    [SerializeField] private Image here;
    [SerializeField] private Image they;
    [SerializeField] private Image are;

    [SerializeField] private Canvas effectsCanvas;

    [SerializeField] private GameObject breakParticle;

    private void Awake()
    {
        main = this;
    }

    [ContextMenu("START DEFENSE")]
    private void CallStart()
    {
        StartCoroutine(StartDefensePhase());
    }

    private IEnumerator StartDefensePhase()
    {
        currentGamePhase = GamePhase.Defense;
        ChangeGamePhase.Invoke();

        /*here.rectTransform.position += Vector3.up * 50;
        they.rectTransform.position += Vector3.up * 50;
        are.rectTransform.position += Vector3.up * 50;*/

        effectsCanvas.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        here.rectTransform.DOAnchorPos(new Vector3(0, 400, 0), .2f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.08f);
        HitFeedback(1);
        yield return new WaitForSeconds(.8f);
        they.rectTransform.DOAnchorPos(new Vector3(0, 0, 0), .2f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.08f);
        HitFeedback(2);
        yield return new WaitForSeconds(.8f);
        are.rectTransform.DOAnchorPos(new Vector3(0, -420, 0), .2f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.08f);
        HitFeedback(3);

        yield return new WaitForSeconds(1f);
        effectsCanvas.GetComponent<CanvasGroup>().DOFade(0, 3f);
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
}

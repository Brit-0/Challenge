using DG.Tweening;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScreen : MonoBehaviour
{
    private CanvasGroup finalCanvasGroup;

    public static float playStartTime, playEndTime;
    public static float timePlayed;
    public static int killedEnemies;
    public static float damageTaken;
    public static int builtTowers;

    [SerializeField] private TMP_Text timePlayedLbl;
    [SerializeField] private TMP_Text killedEnemiesLbl;
    [SerializeField] private TMP_Text damageTakenLbl;
    [SerializeField] private TMP_Text builtTowersLbl;

    private void Awake()
    {
        timePlayed = playEndTime - playStartTime;
        timePlayedLbl.text = TimeSpan.FromSeconds(timePlayed).ToString("hh\\:mm\\:ss"); ;
        killedEnemiesLbl.text = killedEnemies.ToString();
        damageTakenLbl.text = damageTaken + " Corações";
        builtTowersLbl.text = builtTowers.ToString();

        finalCanvasGroup = GetComponent<CanvasGroup>();
        finalCanvasGroup.alpha = 0f;
        finalCanvasGroup.DOFade(1f, 5f);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            finalCanvasGroup.DOFade(0f, 3f).OnComplete(() => { SceneManager.LoadScene("Main Menu"); });
        }
    }
}

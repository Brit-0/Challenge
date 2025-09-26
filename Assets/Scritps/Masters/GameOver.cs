using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private CanvasGroup gameOverCanvasGroup;

    private void Awake()
    {
        gameOverCanvasGroup = GetComponent<CanvasGroup>();
        gameOverCanvasGroup.alpha = 0f;
        gameOverCanvasGroup.DOFade(1f, 5f);
    }

    private void Start()
    {
        AudioManager.main.PlaySound(AudioManager.main.gameOver);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            MainMenu.hasAlreadyDoneTutorial = true;
            gameOverCanvasGroup.DOFade(0f, 3f).OnComplete(() => { SceneManager.LoadScene("Main Menu"); });
        }
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu main;

    [Header("VARIABLES")]
    private bool isTransitioning;
    private RectTransform selectedButton;
    private int selectedIndex;
    private bool isMute;
    private bool isPaused;

    [Header("REFERENCES")]
    [SerializeField] private RectTransform selector;
    [SerializeField] private CanvasGroup pauseCanvasGroup;
    [SerializeField] private AudioListener playerListener;
    [SerializeField] private Image audioButton;
    [SerializeField] private Sprite audioMute;
    [SerializeField] private Sprite audioUnmute;
    [SerializeField] private GameObject pauseButton;

    [Header("BUTTONS")]
    [SerializeField] private RectTransform resumeButton;
    [SerializeField] private RectTransform restartButton;
    [SerializeField] private RectTransform mainMenuButton;
    [SerializeField] private RectTransform quitButton;
    private Dictionary<int, RectTransform> buttonsOrder = new();

    private void Awake()
    {
        main = this;

        buttonsOrder[0] = resumeButton;
        buttonsOrder[1] = restartButton;
        buttonsOrder[2] = mainMenuButton;
        buttonsOrder[3] = quitButton;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SelectManual(true);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SelectManual(false);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            selectedButton.GetComponent<Button>().onClick.Invoke();
        }
    }

    #region SELECT

    public void SelectButton(RectTransform selected)
    {
        if (isTransitioning) return;

        if (!selector.gameObject.activeInHierarchy)
        {
            selector.position = selectedButton.GetChild(1).position;
            selector.gameObject.SetActive(true);

            if (selectedButton == selected)
            {
                AudioManager.main.PlaySoundOneShot(AudioManager.main.select);
                return;
            }
        }

        if (selectedButton == selected) return;

        selectedIndex = GetKeyFromValue(selected);
        selectedButton = selected;
        selector.DOMove(selectedButton.GetChild(1).position, .2f);
        AudioManager.main.PlaySoundOneShot(AudioManager.main.select);
    }

    private void SelectManual(bool next)
    {
        if (isTransitioning) return;

        if (next)
        {
            selectedIndex++;
            if (selectedIndex > 3)
            {
                selectedIndex = 0;
            }
        }
        else
        {
            selectedIndex--;
            if (selectedIndex < 0)
            {
                selectedIndex = 3;
            }
        }

        SelectButton(buttonsOrder[selectedIndex]);
    }

    #endregion

    #region ACTIONS

    public void TogglePause()
    {
        if (isTransitioning) return;

        if (!isPaused) //PAUSE
        {
            isTransitioning = true;
            isPaused = true;

            resumeButton.GetComponent<CanvasGroup>().alpha = 1f;
            restartButton.GetComponent<CanvasGroup>().alpha = 1f;
            mainMenuButton.GetComponent<CanvasGroup>().alpha = 1f;
            quitButton.GetComponent<CanvasGroup>().alpha = 1f;

            resumeButton.localScale = Vector3.one;
            restartButton.localScale = Vector3.one;
            mainMenuButton.localScale = Vector3.one;
            quitButton.localScale = Vector3.one;

            pauseCanvasGroup.alpha = 0f;
            pauseCanvasGroup.gameObject.SetActive(true);
            pauseButton.SetActive(false);
            pauseCanvasGroup.DOFade(1f, .5f).OnComplete(()=> { isTransitioning = false; SelectButton(resumeButton); });
        }
        else //UNPAUSE
        {
            Resume();
        }
        
    }

    public void Resume()
    {
        isPaused = false;
        isTransitioning = true;
        ButtonClick(resumeButton);

        restartButton.GetComponent<CanvasGroup>().DOFade(0f, 1f);
        mainMenuButton.GetComponent<CanvasGroup>().DOFade(0f, 1f);
        quitButton.GetComponent<CanvasGroup>().DOFade(0f, 1f);

        pauseCanvasGroup.DOFade(0f, 2f).OnComplete(() => { pauseCanvasGroup.gameObject.SetActive(false); pauseButton.SetActive(true); isTransitioning = false; });
    }

    public void Restart()
    {
        isTransitioning = true;
        ButtonClick(restartButton);

        mainMenuButton.GetComponent<CanvasGroup>().DOFade(0f, 1f);
        quitButton.GetComponent<CanvasGroup>().DOFade(0f, 1f);
        resumeButton.GetComponent<CanvasGroup>().DOFade(0f, 1f);

        StartCoroutine(AudioManager.FadeOut(AudioManager.main.musicSource, 1.5f));

        StartCoroutine(Resetting());
    }

    public void MainMenu()
    {
        isTransitioning = true;
        ButtonClick(mainMenuButton);

        restartButton.GetComponent<CanvasGroup>().DOFade(0f, 1f);
        quitButton.GetComponent<CanvasGroup>().DOFade(0f, 1f);
        resumeButton.GetComponent<CanvasGroup>().DOFade(0f, 1f);

        StartCoroutine(AudioManager.FadeOut(AudioManager.main.musicSource, 1.5f));

        StartCoroutine(MainMenuCoroutine());
    }

    public void Quit()
    {
        isTransitioning = true;
        ButtonClick(quitButton);

        restartButton.GetComponent<CanvasGroup>().DOFade(0f, 1f);
        mainMenuButton.GetComponent<CanvasGroup>().DOFade(0f, 1f);
        resumeButton.GetComponent<CanvasGroup>().DOFade(0f, 1f);

        StartCoroutine(AudioManager.FadeOut(AudioManager.main.musicSource, 1.5f));

        StartCoroutine(Quitting());
    }

    public void ToggleMute()
    {
        if (isMute) //UNMUTE
        {
            audioButton.sprite = audioUnmute;
            AudioListener.volume = 1;
            isMute = false;
        }
        else //MUTE
        {
            audioButton.sprite = audioMute;
            AudioListener.volume = 0;
            isMute = true;
        }
    }

    #endregion

    #region COROUTINES

    private IEnumerator Resetting()
    {
        yield return new WaitForSeconds(1.5f);

        StaticReset.ResetAll();
        SceneManager.LoadScene("Gameplay");
    }

    private IEnumerator MainMenuCoroutine()
    {
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("Main Menu");
    }

    private IEnumerator Quitting()
    {
        yield return new WaitForSeconds(1.5f);

        Application.Quit();
    }

    #endregion

    #region GENERIC

    private void ButtonClick(RectTransform button)
    {
        selector.gameObject.SetActive(false);

        AudioManager.main.PlaySound(AudioManager.main.click);

        button.DOScale(1.5f, 2f);
        button.GetComponent<CanvasGroup>().DOFade(0f, 2f);
    }

    public int GetKeyFromValue(Transform valueVar)
    {
        foreach (int keyVar in buttonsOrder.Keys)
        {
            if (buttonsOrder[keyVar] == valueVar)
            {
                return keyVar;
            }
        }
        return 0;
    }

    #endregion
}

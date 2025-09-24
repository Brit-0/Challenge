using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("GENERIC")]
    [SerializeField] private RectTransform title;

    [Header("OPTIONS")]
    [SerializeField] private GameObject optionsTab;
    [SerializeField] private TMP_Dropdown resDropdown;
    private Resolution[] resolutions;

    [Header("BUTTONS")]
    [SerializeField] private Transform selector;
    [SerializeField] private Transform playBtn;
    [SerializeField] private Transform optionsBtn;
    [SerializeField] private Transform creditsBtn;
    [SerializeField] private Transform quitBtn;

    [SerializeField] private Transform creditsBackButton;

    [Header("CREDITS")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private CanvasGroup credits;

    private bool isTransitioning;

    
    private Vector3 titlePositionMenu = new Vector3(3.2f, 1.4f, 0f), titlePositionCredits = new Vector3(0f, 1.4f, 0f);

    private Transform selectedButton;

    private void Start()
    {
        SetResDropdown();
    }


    #region SETTINGS

    private void SetResDropdown()
    {
        resolutions = Screen.resolutions;

        resDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            /*if (i != resolutions.Length - 1)
            {
                if (resolutions[i].width == resolutions[i + 1].width &&
                resolutions[i].height == resolutions[i + 1].height)
                {
                    continue;
                }
                else
                {
                    
                }
            }*/
            string option = resolutions[i].width + "x" + resolutions[i].height + " @" + resolutions[i].refreshRateRatio + " hz";
            options.Add(option);

            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resDropdown.AddOptions(options);
        resDropdown.value = currentResolutionIndex;
        resDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    #endregion
    public void SelectButton(Transform selected)
    {
        if (isTransitioning) return;

        if (selectedButton == selected && !selector.gameObject.activeInHierarchy)
        {
            selector.gameObject.SetActive(true);
            AudioManager.main.PlaySoundOneShot(AudioManager.main.select);
            return;
        }

        selectedButton = selected;
        selector.DOMove(selected.position + Vector3.left * .7f + Vector3.down * .11f, .2f);
        AudioManager.main.PlaySoundOneShot(AudioManager.main.select);
    }

    public void Play()
    {
        isTransitioning = true;
        selector.gameObject.SetActive(false);
        AudioManager.main.PlaySound(AudioManager.main.click);

        playBtn.DOScale(1.5f, 3f);
        playBtn.GetComponent<CanvasGroup>().DOFade(0f, 2.5f);

        optionsBtn.GetComponent<CanvasGroup>().DOFade(0f, 1f);
        creditsBtn.GetComponent<CanvasGroup>().DOFade(0f, 1f);
        quitBtn.GetComponent<CanvasGroup>().DOFade(0f, 1f);

        StartCoroutine(AudioManager.FadeOut(AudioManager.main.musicSource, 3f));

        Camera.main.GetComponent<Animator>().enabled = true;
        Camera.main.GetComponent<Animator>().SetTrigger("Play");
    }

    public void Credits()
    {
        SelectButton(creditsBackButton);

        isTransitioning = true;
        selector.gameObject.SetActive(false);
        AudioManager.main.PlaySound(AudioManager.main.click);

        creditsBtn.DOScale(1.5f, 3f).SetLoops(2, LoopType.Yoyo);
        creditsBtn.GetComponent<CanvasGroup>().DOFade(0f, 2.5f);

        optionsBtn.GetComponent<CanvasGroup>().DOFade(0f, 1f);
        playBtn.GetComponent<CanvasGroup>().DOFade(0f, 1f);
        quitBtn.GetComponent<CanvasGroup>().DOFade(0f, 1f);

        creditsPanel.SetActive(true);
        creditsPanel.GetComponent<CanvasGroup>().DOFade(1f, 2f);
        var seq = DOTween.Sequence();
        seq.Append(title.DOMove(titlePositionCredits, 2f));
        seq.Append(credits.DOFade(1f, 2f));
        seq.Append(creditsBackButton.GetComponent<CanvasGroup>().DOFade(1f, 1f)).OnComplete(() => { isTransitioning = false; });
    }

    public void CloseCredits()
    {
        SelectButton(playBtn);
        selector.gameObject.SetActive(false);

        creditsBackButton.DOScale(1.5f, 1.5f).SetLoops(2, LoopType.Yoyo);
        creditsBackButton.GetComponent<CanvasGroup>().DOFade(0f, 1.5f);
        AudioManager.main.PlaySound(AudioManager.main.click);

        var seq = DOTween.Sequence();
        seq.Append(credits.DOFade(0f, 1f));
        seq.Append(creditsPanel.GetComponent<CanvasGroup>().DOFade(0f, 1f).OnComplete(()=> { creditsPanel.SetActive(false); }));

        creditsBtn.GetComponent<CanvasGroup>().DOFade(1f, 2f);
        optionsBtn.GetComponent<CanvasGroup>().DOFade(1f, 2f);
        playBtn.GetComponent<CanvasGroup>().DOFade(1f, 2f);
        quitBtn.GetComponent<CanvasGroup>().DOFade(1f, 2f);

        title.DOMove(titlePositionMenu, 2f);
    }

    public void Quit()
    {
        isTransitioning = true;
        selector.gameObject.SetActive(false);
        AudioManager.main.PlaySound(AudioManager.main.click);

        quitBtn.DOScale(1.5f, 3f);
        quitBtn.GetChild(0).GetComponent<TMP_Text>().DOFade(0f, 2.5f);

        optionsBtn.GetComponent<CanvasGroup>().DOFade(0f, 1f);
        creditsBtn.GetComponent<CanvasGroup>().DOFade(0f, 1f);
        playBtn.GetComponent<CanvasGroup>().DOFade(0f, 1f);

        StartCoroutine(Quitting());
    }

    private IEnumerator Quitting()
    {
        yield return new WaitForSeconds(1.5f);

        Application.Quit();
    }

}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsTab;
    [SerializeField] private TMP_Dropdown resDropdown;
    [SerializeField] private Transform mouseLight;
    private Resolution[] resolutions;
    private UITweener uiTweener;

    private void Awake()
    {
        uiTweener = optionsTab.GetComponent<UITweener>();
    }

    private void Start()
    {
        SetResDropdown();
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mouseLight.position = mousePos;
    }

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

    public void Play()
    {
        SceneManager.LoadScene("Cena DEV");
        PlayerMovement.canMove = true;
    }

    public void SelectDoor()
    {
        AudioManager.main.PlaySound(AudioManager.main.doorOpen);
    }

    public void UnselectDoor()
    {
        AudioManager.main.PlaySound(AudioManager.main.doorClose);
    }

    public void Quit()
    {
        print("Quitou");
        Application.Quit();
    }

}

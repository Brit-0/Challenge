using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : UIHandler
{
    private UITweener uiTweener;


    public void OnClick()
    {
        uiTweener = GetComponent<UITweener>();
        ToggleTab(gameObject);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    protected override bool ToggleTab(GameObject obj, bool hasPopUp = false, float scale = 0, UITweener uITweener = null, bool blockMovement = false)
    {
        bool opened = base.ToggleTab(obj, hasPopUp, scale, uITweener, blockMovement);

        if (opened)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        return opened;
    }
}

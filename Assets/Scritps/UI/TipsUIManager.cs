using System.Collections;
using TMPro;
using UnityEngine;

public class TipsUIManager : MonoBehaviour
{
    public TextMeshProUGUI tipsLbl;
    [SerializeField] private GameObject tipsPanel;
    public static TipsUIManager current;

    [SerializeField] float delay;
    private string currentText = "";

    private void Awake()
    {
        current = this;
    }

    public void SetTip(string text)
    {
        tipsLbl.text = text;
        tipsLbl.gameObject.SetActive(true);
    }

    public void DisableTip()
    {
        tipsLbl.gameObject.SetActive(false);
    }

    public void WriteTip(string text)
    {
        StartCoroutine(ShowText(text));
    }

    private IEnumerator ShowText(string fullText)
    {
        Tutorial.canAdvance = false;

        for (int i = 0; i < fullText.Length + 1; i++)
        {
            currentText = fullText.Substring(0, i);
            tipsLbl.text = currentText;
            yield return new WaitForSeconds(delay);
        }

        Tutorial.canAdvance = true;
    }
}

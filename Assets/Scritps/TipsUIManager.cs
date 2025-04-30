using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TipsUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tipsLbl;
    [SerializeField] private GameObject tipsPanel;
    public static TipsUIManager current;

    private void Awake()
    {
        current = this;
    }

    public void setTip(string text)
    {
        tipsPanel.GetComponent<Image>().color = tipsPanel.GetComponent<Image>().color.WithAlpha(.6f);
        tipsLbl.text = text;
        tipsLbl.gameObject.SetActive(true);
    }

    public void disableTip()
    {
        tipsPanel.GetComponent<Image>().color = tipsPanel.GetComponent<Image>().color.WithAlpha(0f);
        tipsLbl.gameObject.SetActive(false);
    }
}

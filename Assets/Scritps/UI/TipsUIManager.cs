using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class TipsUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tipsLbl;
    [SerializeField] private GameObject tipsPanel;
    public static TipsUIManager current;

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
}

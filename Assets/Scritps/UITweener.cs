using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class UITweener : MonoBehaviour
{
    [SerializeField] CraftingTabUIManager craftingTab;
    [SerializeField] RectTransform rectTrans;

    private void Callback(object tab)
    {
        if ((string)tab == "crafting")
        {
            craftingTab.CloseTab();
        }
    }

    public void CloseTween(string tab)
    {
        LeanTween.scale(rectTrans, Vector2.zero, .2f).setOnComplete(Callback).setOnCompleteParam(tab);
    }

    public void PopUpTween()
    {
        LeanTween.scale(rectTrans, new Vector3(.2f, .2f, .2f), .2f);
    }

    public void ClickTween(float clickTime)
    {
        if (!LeanTween.isTweening(rectTrans))
        {
            /*
            var scaleSeq = LeanTween.sequence();
            scaleSeq.append(LeanTween.scale(rectTrans, rectTrans.localScale * 0.8f, .05f));
            scaleSeq.append(LeanTween.scale(rectTrans, rectTrans.localScale, .05f));
            */
            LeanTween.scale(rectTrans, rectTrans.localScale * 0.8f, clickTime).setLoopPingPong(1);
        }
    }

    public void ColorFlickTween(Color color, float speed)
    {
        LeanTween.color(rectTrans, color, speed).setLoopPingPong(1);
    }

    public int MoveTween(float end, float speed)
    {
        int id = LeanTween.moveLocalX(gameObject, end, .5f).setLoopPingPong().id;
        return id;
    }
}

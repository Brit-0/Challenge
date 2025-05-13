using System;
using UnityEngine;
using UnityEngine.Events;

public class UITweener : MonoBehaviour
{
    RectTransform rectTrans;

    //public UnityEvent closeTab; //DEFINIR NO INSPECTOR

    private void Awake()
    {
        rectTrans = GetComponent<RectTransform>();
    }

    public void CloseTween()
    {
        LeanTween.scale(rectTrans, Vector2.zero, .2f);
    }

    public void PopUpTween(float scale)
    {
        LeanTween.scale(rectTrans, new Vector3(scale, scale, scale), .2f);
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

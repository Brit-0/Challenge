using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScreen : MonoBehaviour
{
    private CanvasGroup finalCanvasGroup;

    private void Awake()
    {
        finalCanvasGroup = GetComponent<CanvasGroup>();
        finalCanvasGroup.DOFade(1f, 5f);
    }
}

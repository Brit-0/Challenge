using System;
using UnityEngine;
using UnityEngine.Events;

public class UIHandler : MonoBehaviour
{
    public static bool isTabOpened;

    protected virtual bool ToggleTab(GameObject obj, bool hasPopUp = default(bool), float scale = default(int), UITweener uITweener = null, bool blockMovement = default(bool))
    {
        if (obj.activeInHierarchy) //FECHAR
        {
            if (hasPopUp) //TEM ANIMAÇÃO OU NÃO
            {
                var seq = LeanTween.sequence();
                seq.append(() => { uITweener.CloseTween(); });
                seq.append(.2f);
                seq.append(() =>
                {
                    UIHandler.isTabOpened = false;
                    obj.SetActive(false);
                    if (blockMovement)
                    {
                        PlayerMovement.canMove = true;
                    }
                });
            }
            else
            {
                obj.SetActive(false);
            }

            return false;
        }
        else //ABRIR
        {
            if (hasPopUp && uITweener) //TEM ANIMAÇÃO OU NÃO
            {
                obj.transform.localScale = Vector3.zero;
                obj.SetActive(true);
                uITweener.PopUpTween(scale);
            }
            else
            {
                obj.SetActive(true);
            }

            if (blockMovement)
            {
                PlayerMovement.canMove = false;
            }

            isTabOpened = true;
            return true;
        }
    }
}   

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCheck : MonoBehaviour
{
    [SerializeField] GameObject skillCheck;
    [SerializeField] RectTransform sc_check, sc_success;
    [SerializeField] UITweener uiTweener;

    private float randomWidth, randomPosX;
    private bool success;
    [SerializeField] int successNeeded;
    private int id;
    private int successCounter;

    private void Start()
    { 
        SetSuccess();
        id = uiTweener.MoveTween(.97f, .5f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine("CheckSuccess");
        }
    }

    private void SetSuccess()
    {
        randomWidth = Random.Range(.1f, .6f);
        randomPosX = Random.Range(randomWidth / 2, 2 - (randomWidth / 2));
        sc_success.sizeDelta = new Vector2(randomWidth, sc_success.sizeDelta.y);
        sc_success.anchoredPosition = new Vector2(randomPosX, sc_success.anchoredPosition.y);
        sc_success.GetComponent<BoxCollider2D>().size = new Vector2(randomWidth, sc_success.GetComponent<BoxCollider2D>().size.y);
    }

    private IEnumerator CheckSuccess()
    {
        LeanTween.pause(id);
        uiTweener.ColorFlickTween(Color.black, .25f);

        yield return new WaitForSecondsRealtime(.1f);

        if (success)
        {
            successCounter++;
            print("Successos: " + successCounter + "/" + successNeeded);
        }

        yield return new WaitForSecondsRealtime(.5f);

        if (successCounter == successNeeded)
        {
            Destroy(skillCheck);
        }

        ResetCheck();
    }

    private void ResetCheck()
    {
        SetSuccess();
        LeanTween.resume(id);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == sc_success.name)
        {
            success = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == sc_success.name)
        {
            success = false;
        }
    }
}

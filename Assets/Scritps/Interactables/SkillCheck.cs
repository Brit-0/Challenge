using System.Collections;
using UnityEngine;

public class SkillCheck : MonoBehaviour
{
    [SerializeField] GameObject skillCheck;
    [SerializeField] RectTransform sc_check, sc_success;
    [SerializeField] UITweener uiTweener;
    [SerializeField] Animator animator;

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
            animator.SetTrigger("Success");
            //print("Successos: " + successCounter + "/" + successNeeded);
        }
        else
        {
            animator.SetTrigger("Failure");
        }

        if (successCounter == successNeeded)
        {
            AudioManager.main.PlaySound(AudioManager.main.chestOpen, .8f);

            yield return new WaitForSecondsRealtime(.5f);

            ChestScript.currentChest.Open();
            TipsUIManager.current.DisableTip();
            PlayerMovement.canMove = true;

            Destroy(skillCheck);
        }
        else
        {
            AudioManager.main.PlaySound(AudioManager.main.lockpick, .2f);

            yield return new WaitForSecondsRealtime(.5f);
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

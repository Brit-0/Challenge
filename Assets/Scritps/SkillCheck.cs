using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCheck : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] RectTransform sc_check, sc_success;

    private float randomWidth, randomPosX;
    private bool success;
    [SerializeField] int successNeeded;
    private int successCounter;

    private void Start()
    {
        SetSuccess();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckSucces();
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

    private void CheckSucces()
    {
        animator.SetTrigger("Stop");

        if (success)
        {
            successCounter++;
            print("Successos: " + successCounter + "/" + successNeeded);

            if (successCounter == successNeeded)
            {
                print("Baú Aberto");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.name);

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

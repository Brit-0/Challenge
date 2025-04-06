using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class TowerLogic : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private TowerData towerData;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject buttons;
    [SerializeField] private SpriteRenderer _renderer;


    private int currentHealth;
    [SerializeField] private int towerLvl = 1, maxLvl;


    private void Start()
    {
        currentHealth = towerData.maxHealth;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (TowerManager.placeMode) return;

        if (TowerManager.selected != this && towerLvl < 4)
        {
            if (TowerManager.selected != null)
            {
                TowerManager.selected.ToggleMenu();
            }

            buttons.SetActive(true);
            TowerManager.selected = this;
        }
        else
        {
            buttons.SetActive(false);
            TowerManager.selected = null;
        }
        
    }

    public void UpgradeTower()
    {
        towerLvl++;
        animator.SetTrigger("LevelUp");
        _renderer.material.SetTexture("_MainTexture", _renderer.sprite.texture);

        if (towerLvl == maxLvl)
        {
            ToggleMenu();
        }
    }

    public void SetRandomSkin()
    {
        float randomH = Random.Range(1, 361) / 360f;
        float randomS = Random.Range(30, 101) / 100f;
        float randomV = Random.Range(40, 101) / 100f;
        print("H: " + randomH + " S: " + randomS + " V: " + randomV);
        _renderer.material.SetColor("_SkinColor", Color.HSVToRGB(randomH, randomS, randomV));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Tower"))
        {
            TowerManager.isColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Tower"))
        {
            TowerManager.isColliding = false;
        }
    }
}

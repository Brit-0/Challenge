using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerLogic : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private TowerData towerData;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject upgradeButton;

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

            upgradeButton.SetActive(true);
            TowerManager.selected = this;
        }
        else
        {
            upgradeButton.SetActive(false);
            TowerManager.selected = null;
        }
        
    }

    public void UpgradeTower()
    {
        //transform.localScale += new Vector3(.1f, .1f, .1f);
        towerLvl++;
        animator.SetTrigger("LevelUp");

        if (towerLvl == maxLvl)
        {
            ToggleMenu();
        }
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

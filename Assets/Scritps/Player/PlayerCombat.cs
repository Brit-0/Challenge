using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat main;

    [SerializeField] private SpriteRenderer bloodVignete;

    [SerializeField] private float maxHearts = 5f;
    [SerializeField] private float currentHearts;

    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;

    [SerializeField] private Image[] hearts = new Image[5];

    [SerializeField] private Image dmgBlocker;

    private float takeDamageCooldown = .5f;
    private float nextTimeToTakeDamage;

    [SerializeField] private float timeToHeal;
    private float currentHealTime;
    [SerializeField] private Image healSlider;
    [SerializeField] private GameObject healIcon;
    public Coroutine healingCoroutine;
    [SerializeField] private Image healBlocker;
    public Image healItemBG;

    private int lastHeartIndex = 5;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currentHearts = maxHearts;
    }

    public void TakeDamage(float amount)
    {
        if (Time.time < nextTimeToTakeDamage) return;

        FinalScreen.damageTaken += amount;
        nextTimeToTakeDamage = Time.time + takeDamageCooldown;

        CameraController.main.CameraShake(3f, .2f);

        var seq = DOTween.Sequence();
        seq.Append(dmgBlocker.DOFade(.1f, .2f));
        seq.Append(dmgBlocker.DOFade(0f, .2f));

        currentHearts -= amount;
        bloodVignete.DOFade((-25 * currentHearts + 125) * 0.01f, .2f);
        StartCoroutine(ImpactHit());

        if (currentHearts % 1 == 0)
        {
            hearts[lastHeartIndex - 1].sprite = emptyHeart;
        }
        else
        {
            if (Mathf.CeilToInt(currentHearts) == lastHeartIndex)
            {
                hearts[lastHeartIndex - 1].sprite = halfHeart;
            }
            else
            {
                hearts[lastHeartIndex - 1].sprite = emptyHeart;
                lastHeartIndex--;
                hearts[lastHeartIndex - 1].sprite = halfHeart;
            }
        }

        lastHeartIndex = Mathf.CeilToInt(currentHearts);
    }

    public IEnumerator ImpactHit()
    {
        yield return new WaitForSeconds(.1f);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(.05f);
        Time.timeScale = 1;
    }

    #region HEAL

    public IEnumerator StartHealing()
    {
        PlayerInput.blockInput = true;
        healItemBG.color = new Color32(150, 150, 150, 90);
        PlayerMovement.main.BlockMovement();
        healIcon.SetActive(true);

        while (currentHealTime < timeToHeal)
        {
            currentHealTime += Time.deltaTime;
            healSlider.fillAmount = currentHealTime / timeToHeal;
            yield return null;
        }

        Heal();
        PlayerInput.isHealing = false;
        ResetHealing();
    }

    public void ResetHealing()
    {
        PlayerInput.blockInput = false;
        healItemBG.color = new Color32(50, 50, 50, 90);
        healIcon.SetActive(false);
        currentHealTime = 0f;
        healSlider.fillAmount = 0f;
        PlayerMovement.canMove = true;
        StopCoroutine(healingCoroutine);
    }

    private void Heal()
    {
        PlayerInventory.current.ChangeBandages(-1);
        healBlocker.DOFade(.1f, .15f).SetLoops(2, LoopType.Yoyo);
        currentHearts += 1;
        currentHearts = Mathf.Clamp(currentHearts, 0, 5);
        bloodVignete.DOFade((-25 * currentHearts + 125) * 0.01f, .2f);

        if (currentHearts == 5)
        {
            hearts[4].sprite = fullHeart;
            lastHeartIndex = 5;
            return;
        }

        if (currentHearts % 1 == 0)
        {
            hearts[lastHeartIndex].sprite = fullHeart;
        }
        else
        {
            hearts[lastHeartIndex - 1].sprite = fullHeart;
            hearts[lastHeartIndex].sprite = halfHeart;
        }

        lastHeartIndex = Mathf.CeilToInt(currentHearts);
    }

    #endregion

    public float GetHearts() { return currentHearts; }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Altar : Interactable
{
    public static Altar main;

    [SerializeField] private float currentAltarHealth, sliderAltarHealth, maxAltarHealth = 10;
    [SerializeField] private Slider altarHealthSlider;
    private float currentVelocity = 0f;
    [SerializeField] private bool isUpdatingBar;
    private bool hasLost;
    private bool alreadyInteracted;

    [SerializeField] private GameObject shockwavePf;
    private GameObject currentShockwave;
    private Coroutine shockwaveCoroutine;
    private float shockWaveTime = 5f;
    private Material material;
    private static int waveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFromCenter");
    private bool isShocking;

    private void Awake()
    {
        main = this;

        tip = "Aperte \"E\" para ativar a relíquia";

        currentAltarHealth = maxAltarHealth;
        altarHealthSlider.maxValue = maxAltarHealth;
        altarHealthSlider.value = altarHealthSlider.maxValue;
    }

    protected override void Update()
    {
        if (!alreadyInteracted)
        {
            base.Update();
        }

        if (isUpdatingBar)
        {
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        sliderAltarHealth = Mathf.SmoothDamp(altarHealthSlider.value, currentAltarHealth, ref currentVelocity, 100 * Time.deltaTime);
        altarHealthSlider.value = sliderAltarHealth;

        if (Mathf.Abs(altarHealthSlider.value - currentAltarHealth) < .01f) 
        {
            isUpdatingBar = false;
        }
    }

    public void TakeDamage(float amount)
    {
        if (hasLost) return;

        AudioManager.main.PlaySpatialSound(AudioManager.main.altarHit, gameObject);
        currentAltarHealth -= amount;
        isUpdatingBar = true;

        if (isShocking)
        {
            StopCoroutine(shockwaveCoroutine);
            Destroy(currentShockwave);
        }
        
        shockwaveCoroutine = StartCoroutine(Shockwave());


        if (currentAltarHealth <= 0)
        {
            StartCoroutine(PlayerCombat.main.Die());
            hasLost = true;
        }
     }
  

    protected override void Interact()
    {
        alreadyInteracted = true;
        TipsUIManager.current.DisableTip();
        GameManager.main.StartCoroutine(GameManager.main.StartDefensePhase());
    }

    private IEnumerator Shockwave()
    {
        print("shocked");
        isShocking = true;

        currentShockwave = Instantiate(shockwavePf, transform.position, Quaternion.identity);
        material = currentShockwave.GetComponent<SpriteRenderer>().material;

        material.SetFloat(waveDistanceFromCenter, 0f);

        float lerpedAmount = 0f;

        float elapsedTime = 0f;

        while(elapsedTime < shockWaveTime)
        {
            elapsedTime += Time.deltaTime;

            lerpedAmount = Mathf.Lerp(0f, 1f, (elapsedTime / shockWaveTime));
            material.SetFloat(waveDistanceFromCenter, lerpedAmount);

            yield return null;
        }

        Destroy(currentShockwave);

        isShocking = false;
    }
}

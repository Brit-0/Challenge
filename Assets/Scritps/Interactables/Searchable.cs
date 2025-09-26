using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Searchable : Interactable
{
    [SerializeField] private bool alreadySearched;
    private float currentSearchTime = 0f, timeToSearch = 3f;
    private Image actionSlider;
    public Coroutine searchCoroutine;

    [SerializeField] private GameObject bandagePf;

    private void Awake()
    {
        tip = "Segure \"E\" para procurar bandagens";
    }

    private void Start()
    {
        actionSlider = GameObject.Find("ActionSlider").GetComponent<Image>();
    }

    protected override void Update()
    {
        if (alreadySearched) return;
        
        base.Update();

        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            searchCoroutine = StartCoroutine(StartSearching());
        }

        if (PlayerInput.isSearching && PlayerInput.searchingItem == this && Input.GetKeyUp(KeyCode.E))
        {
            ResetSearch();
        }
    }

    private IEnumerator StartSearching()
    {
        AudioManager.main.PlaySpatialSound(AudioManager.main.searching, gameObject);
        PlayerInput.blockInput = true;
        PlayerInput.isSearching = true;
        PlayerInput.searchingItem = this;
        PlayerMovement.main.BlockMovement();

        while (currentSearchTime < timeToSearch)
        {
            currentSearchTime += Time.deltaTime;
            actionSlider.fillAmount = currentSearchTime / timeToSearch;
            yield return null;
        }

        Search();
        ResetSearch();
    }

    public void ResetSearch()
    {
        PlayerInput.isSearching = false;
        Destroy(GetComponent<AudioSource>());
        PlayerInput.blockInput = false;
        currentSearchTime = 0f;
        actionSlider.fillAmount = 0f;
        PlayerMovement.canMove = true;
        StopCoroutine(searchCoroutine);
    }

    private void Search()
    {
        int lootChance = Random.Range(1, 11);
        int bandages = 0;

        if (lootChance == 10)
        {
            bandages = 2;
        }
        else if (lootChance > 5)
        {
            bandages = 1;
        }

        for (int i = 0; i < bandages; i++)
        {
            GameObject newMaterial = Instantiate(bandagePf, transform.position, Quaternion.identity);
            newMaterial.GetComponent<Rigidbody2D>().velocity = Vector3.up * 3f + new Vector3(Random.Range(-1, 2), 0, 0);
            Destroy(newMaterial.GetComponent<Rigidbody2D>(), .8f);
            //Vector2 pos = transform.position + Vector3.down + new Vector3(Random.Range(-1, 2),0 , 0);
            //newMaterial.transform.DOMove(pos, .5f).SetEase(Ease.OutSine);         
        }

        TipsUIManager.current.DisableTip();
        alreadySearched = true;
    }
}

using UnityEngine;

public class Spawner : Interactable
{
    [SerializeField] private bool isOpen = true;
    public bool isActive;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        tip = "Aperte \"E\" para bloquear a saída";
    }

    protected override void Update()
    {
        if (isOpen && GameManager.currentGamePhase == GamePhase.Defense)
        {
            base.Update();
        }
    }

    protected override void Interact()
    {
        TipsUIManager.current.DisableTip();
        transform.GetChild(0).gameObject.SetActive(true);
        AudioManager.main.PlaySound(AudioManager.main.barricade);
        isOpen = false;
        print(isOpen);
    }
}

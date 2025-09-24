using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public static bool canAdvance;
    private bool isAwaiting;

    [Header("AUDIO")]
    public List<AudioClip> whispers;

    [Header("REFERENCES")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Image blackout;

    [Header("SPAWN")]
    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject torch;
    [SerializeField] private GameObject gate;
    [SerializeField] private GameObject lever;
    [SerializeField] private GameObject tower;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject shelves;
    [SerializeField] private GameObject bandage;
    [SerializeField] private CanvasGroup itemsUI;

    private List<int> awaitingIndexes = new()
    {
        2,
        8,
        13,
        17,
        21
    };

    private List<string> tips = new()
    {
        "Seja bem vindo à masmorra de Here They Are", //0
        "Rapidamente te ensinarei o básico para sua sobrevivência",
        "Essa caveira é seu companheiro fiel. Clique com o botão direto do mouse para utilizá-lo",
        "Vejo que já pegou o jeito. Com isso poderá destruir seus inimigos facilmente",
        "A primeira parte do seu objetivo é encontrar a grande relíquia guardada em uma sala escondida",
        "Porém, assim que alcançá-la, as criaturas saberão, e começarão a tentar destruí-la",
        "As criaturas invadem a masmorra através de buracos como esse",
        "Para impedir, você deve bloquear essas entradas com barricadas",
        "Tente bloquear utilizando a tecla de interação \"E\"",
        "Assim que começar a denfender a relíquia, seu objetivo final é bloquear todas essas entradas, deixando a relíquia segura",
        "Para te auxiliar, você deve pode construir torres de defesa estrategicamente com os recursos que conseguir nos baús", //10
        "Elas atirarão nas criaturas te ajudando a manter a sala final segura",
        "A masmorra pode ser confusa e escura de atravessar, portanto atirar nas tochas apagadas pode ser uma boa ideia",
        "Agora tente você mesmo",
        "Essa também é uma boa forma de marcar o caminho que já percorreu",
        "Durante a exploração, você provavelmente irá se ferir, mas isso não é motivo para desespero", //15
        "Você pode encontrar bandagens nesses móveis, cheios de entulhos e antiguidades",
        "Pegue uma e se cure segurando a tecla \"Q\"",
        "Por fim, a masmorra é cheia de salas bloqueadas por grandes portões como esses",
        "Para abrí-los, basta puxar a alavanca correspondente, apenas não me pergunte onde elas ficam",
        "Por agora, eu te digo: a deste portão é aquela",
        "Ande até lá com as teclas WASD e ative-a",
        "Agora você está pronto para explorar a masmorra, mas tome muito cuidado",
        "Pois ELES ESTÃO AQUI" //20
    };

    private int currentTipIndex;

    private void Awake()
    {
        blackout.color = new Color(0f, 0f, 0f, 1f);
        blackout.DOFade(0f, 4f);
    }

    private void Start()
    {
        TipsUIManager.current.WriteTip("Bem vindo à masmorra de Here They Are");
        AudioManager.main.PlaySound(whispers[Random.Range(0, whispers.Count)], .3f);
        PlayerMovement.canMove = false;
    }

    private void Update()
    {
        if (!canAdvance) return;

        if (!isAwaiting)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                AdvanceTip();
            }   
        }
        else
        {
            switch (currentTipIndex)
            {
                case 2:
                    if (Input.GetMouseButtonDown(1))
                    {
                        AdvanceTip();
                    }

                    break;

                case 8:
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        spawner.transform.GetChild(0).gameObject.SetActive(true);
                        AudioManager.main.PlaySound(AudioManager.main.barricade, .2f);
                        AdvanceTip();
                    }

                    break;

                case 13:
                    if (torch.GetComponent<Animator>().GetBool("isOn"))
                    {
                        AdvanceTip();
                    }

                    break;

                case 17:
                    if (PlayerCombat.main.GetHearts() == 5)
                    {
                        AdvanceTip();
                    }

                    break;

                case 21:
                    if (lever.GetComponent<Animator>().GetBool("isDown"))
                    {
                        AdvanceTip();
                    }

                    break;
            }
        }
    }

    private void AdvanceTip()
    {
        AudioManager.main.PlaySound(whispers[Random.Range(0, whispers.Count)], .3f);

        isAwaiting = false;
        currentTipIndex++;
        TipsUIManager.current.WriteTip(tips[currentTipIndex]);

        if (awaitingIndexes.Contains(currentTipIndex))
        {
            isAwaiting = true;
        }

        //SPAWNS
        switch (currentTipIndex)
        {
            case 6:
                ShowObject(spawner);
                spawner.GetComponent<Animator>().SetBool("isOpen", true);

                break;

            case 10:
                Destroy(spawner);
                ShowObject(tower);

                break;

            case 11:
                ShowObject(enemy);
                StartCoroutine(tower.GetComponent<TorreDeOssos>().SetActive());

                break;

            case 12:
                ShowObject(torch);
                Destroy(tower);
                Destroy(enemy);

                GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");

                foreach (GameObject projectile in projectiles)
                {
                    Destroy(projectile);
                }

                break;

            case 15:
                Destroy(torch);

                break;

            case 16:
                ShowObject(shelves);

                break;

            case 17:
                PlayerCombat.main.TakeDamage(1);
                ShowObject(bandage);
                itemsUI.DOFade(1f, 1.5f);
                Destroy(shelves);

                break;

            case 18:
                ShowObject(gate);

                break;

            case 20:
                ShowObject(lever);

                break;

            case 21:
                PlayerMovement.canMove = true;

                break;

            case 22:
                TipsUIManager.current.tipsLbl.gameObject.SetActive(true);
                PlayerMovement.canMove = false;

                break;

            case 23:
                blackout.DOFade(1f, 5f).OnComplete(StartGame);

                break;
        }
    }

    private void ShowObject(GameObject obj, bool destroy = false)
    {
        obj.SetActive(true);

        if (destroy)
        {
            Destroy(obj, 5f);
        }
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
        MainMenu.hasAlreadyDoneTutorial = true;
        PlayerMovement.canMove = true;
    }
}

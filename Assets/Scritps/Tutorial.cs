using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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

    private List<int> awaitingIndexes = new()
    {
        2,
        8,
        13,
        18
    };

    private List<string> tips = new()
    {
        "Seja bem vindo � masmorra de Here They Are", //0
        "Rapidamente te ensinarei o b�sico para sua sobreviv�ncia",
        "Essa caveira � seu companheiro fiel. Clique com o bot�o direto do mouse para utiliz�-lo",
        "Vejo que j� pegou o jeito. Com isso poder� destruir seus inimigos facilmente",
        "A primeira parte do seu objetivo � encontrar a grande rel�quia guardada em uma sala escondida",
        "Por�m, assim que alcan��-la, as criaturas saber�o, e come�ar�o a tentar destru�-la",
        "As criaturas invadem a masmorra atrav�s de buracos como esse",
        "Para impedir, voc� deve bloquear essas entradas com barricadas",
        "Tente bloquear utilizando a tecla de intera��o \"E\"",
        "Assim que come�ar a denfender a rel�quia, seu objetivo final � bloquear todas essas entradas, deixando a rel�quia segura",
        "Para te auxiliar, voc� deve pode construir torres de defesa estrategicamente com os recursos que conseguir nos ba�s", //10
        "Elas atirar�o nas criaturas te ajudando a manter a sala final segura",
        "A masmorra pode ser confusa e escura de atravessar, portanto atirar nas tochas apagadas pode ser uma boa ideia",
        "Agora tente voc� mesmo",
        "Essa tamb�m � uma boa forma de marcar o caminho que j� percorreu",
        "Por fim, a masmorra � cheia de salas bloqueadas por grandes port�es como esses",
        "Para abr�-los, basta puxar a alavanca correspondente, apenas n�o me pergunte onde elas ficam",
        "Por agora, eu te digo: a deste port�o � aquela",
        "Ande at� l� com as teclas WASD e ative-a",
        "Agora voc� est� pronto para explorar a masmorra, mas tome muito cuidado",
        "Pois ELES EST�O AQUI" //20
    };

    private int currentTipIndex;

    private void Start()
    {
        TipsUIManager.current.WriteTip("Bem vindo � masmorra de Here They Are");
        AudioManager.main.PlaySound(whispers[Random.Range(0, whispers.Count)], .3f);
        PlayerMovement.canMove = false;
    }

    private void Update()
    {
        if (!canAdvance) return;

        if (!isAwaiting)
        {
            if (Input.GetMouseButtonDown(0))
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
                        AdvanceTip();
                    }

                    break;

                case 13:
                    if (torch.GetComponent<Animator>().GetBool("isOn"))
                    {
                        AdvanceTip();
                    }

                    break;

                case 18:
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
                ShowObject(spawner, true);

                break;

            case 12:
                ShowObject(torch);

                break;

            case 15:
                Destroy(torch);
                ShowObject(gate);

                break;

            case 17:
                ShowObject(lever);

                break;

            case 18:
                PlayerMovement.canMove = true;

                break;

            case 19:
                TipsUIManager.current.tipsLbl.gameObject.SetActive(true);
                PlayerMovement.canMove = false;

                break;

            case 20:
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
        PlayerMovement.canMove = true;
    }
}

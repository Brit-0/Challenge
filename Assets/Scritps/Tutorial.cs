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
        "Por fim, a masmorra é cheia de salas bloqueadas por grandes portões como esses",
        "Para abrí-los, basta puxar a alavanca correspondente, apenas não me pergunte onde elas ficam",
        "Por agora, eu te digo: a deste portão é aquela",
        "Ande até lá com as teclas WASD e ative-a",
        "Agora você está pronto para explorar a masmorra, mas tome muito cuidado",
        "Pois ELES ESTÃO AQUI" //20
    };

    private int currentTipIndex;

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

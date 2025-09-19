using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bloodVignete;

    [SerializeField] private float maxHearts = 5f;
    [SerializeField] private float currentHearts;

    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;

    [SerializeField] private Image[] hearts = new Image[5];

    private int lastHeartIndex = 5;

    private void Start()
    {
        currentHearts = maxHearts;
    }

    public void TakeDamage(float amount)
    {
        print("Damage");
        CameraController.main.CameraShake(3f, .3f);
        currentHearts -= amount;
        bloodVignete.DOFade((-25 * currentHearts + 125) * 0.01f, .2f);

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
        print(lastHeartIndex);
    }
}

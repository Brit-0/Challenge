using UnityEngine;

public class MenuBG : MonoBehaviour
{
    private Vector2 startPos;
    [SerializeField] private float moveModifier;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        Vector2 mPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float posY = Mathf.Lerp(transform.position.y, startPos.y + (mPos.y * -moveModifier), 2f * Time.deltaTime);

        transform.position = new Vector3(0, posY, 0);
    }
}

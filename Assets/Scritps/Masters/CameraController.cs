using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region CAMERA SHAKE

    public IEnumerator CameraShake(float strength, float time)
    {
        Vector3 preShakeAngle = transform.localEulerAngles;
        float strengthVelocity = 0f;

        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            strength = Mathf.SmoothDamp(strength, 0f, ref strengthVelocity, time);

            float randomX = Random.value - 0.5f;
            float randomY = Random.value - 0.5f;

            transform.localEulerAngles = new Vector3(randomX, randomY, 0) * strength;

            yield return null;
        }

        transform.localEulerAngles = preShakeAngle;
    }

    #endregion
}

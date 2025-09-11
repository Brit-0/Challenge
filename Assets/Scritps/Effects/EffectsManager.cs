using System.Collections;
using UnityEngine;

public enum Particle
{
    Impact
}

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager main;

    [SerializeField] GameObject impactPf;

    private void Awake()
    {
        main = this;
    }

    public void CreateParticle(Particle particle, Vector2 pos)
    {
        GameObject prefab = default;

        switch (particle)
        {
            case Particle.Impact:
                prefab = impactPf;

                break;
        }

        GameObject newParticle = Instantiate(prefab, pos, Quaternion.identity);
        StartCoroutine(DestroyParticle(newParticle));
    }

    public IEnumerator DestroyParticle(GameObject particle)
    {
        yield return new WaitForSeconds(1f);

        Destroy(particle);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class SkelLogic : MonoBehaviour
{
    public static SkelLogic main;

    private NavMeshAgent navAgent;
    private Vector2 randomPoint;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject projectilePf;
    [SerializeField] private Light2D eye1, eye2;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;

        StartCoroutine(SetRandomOffset());
    }

    private void Update()
    {
        navAgent.SetDestination((Vector2)playerTransform.position + randomPoint);
    }

    private IEnumerator SetRandomOffset()
    {
       randomPoint = Random.onUnitSphere;

       yield return new WaitForSeconds(3f);

       StartCoroutine(SetRandomOffset());
    }

    public void Shoot()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject newProjectile = Instantiate(projectilePf, transform.position, Quaternion.identity);
        newProjectile.GetComponent<ProjectileLogic>().SetDataSkel(mousePos);

        AudioManager.main.PlaySound(AudioManager.main.shoot);

        StartCoroutine(EyeFeedback());
    }

    private IEnumerator EyeFeedback()
    {
        eye1.intensity = 0;
        eye2.intensity = 0;

        yield return new WaitForSeconds(1.5f);

        eye1.intensity = 8;
        eye2.intensity = 8;
    }

}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SkelLogic : MonoBehaviour
{
    public static SkelLogic main;

    private NavMeshAgent navAgent;
    private Vector2 randomPoint;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject projectilePf;

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
    }

}

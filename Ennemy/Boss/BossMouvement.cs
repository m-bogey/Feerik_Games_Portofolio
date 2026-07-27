using UnityEngine;
using UnityEngine.AI;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform moveZone;

    public bool HasReachedDestination =>
        !agent.pathPending &&
        agent.remainingDistance <= agent.stoppingDistance;

    public void MoveToRandomPoint()
    {
        Vector3 randomPos = GetRandomPoint();
        agent.SetDestination(randomPos);
    }

    private Vector3 GetRandomPoint()
    {
        Vector3 center = moveZone.position;
        Vector3 size = moveZone.localScale * 10;

        float randomX = Random.Range(-size.x / 2, size.x / 2);
        float randomZ = Random.Range(-size.z / 2, size.z / 2);

        return center + new Vector3(randomX, 0, randomZ);
    }
}
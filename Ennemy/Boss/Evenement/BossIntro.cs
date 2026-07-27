using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossIntro : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    //[SerializeField] private Animator animator;

    //pour cacher le boss
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Collider[] colliders;

    [Header("Intro")]
    // pour faire deplacer le boss au debut
    [SerializeField] private Transform introPoint;

    [Header("end")]
    // pour faire deplacer le boss à la fin
    [SerializeField] private Transform endPoint;

    private void Awake()
    {
        // desactive les renderers du boss
        foreach (Renderer r in renderers)
            r.enabled = false;
        // desactive les colliders du boss
        foreach (Collider c in colliders)
            c.enabled = false;
    }

    public IEnumerator PlayIntro()
    {
        Debug.Log("Début intro");
        // active les renderers du boss
        foreach (Renderer r in renderers)
            r.enabled = true;
        // active les colliders du boss
        foreach (Collider c in colliders)
            c.enabled = true;

        agent.SetDestination(introPoint.position);

        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            yield return null;

        //animator.SetTrigger("Roar");

        yield return new WaitForSeconds(2f);

        Debug.Log("Fin intro");
    }

    public IEnumerator PlayEnd()
    {
        agent.SetDestination(endPoint.position);

        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            yield return null;

        // desactive les renderers du boss
        foreach (Renderer r in renderers)
            r.enabled = false;
        // desactive les colliders du boss
        foreach (Collider c in colliders)
            c.enabled = false;
    }
}
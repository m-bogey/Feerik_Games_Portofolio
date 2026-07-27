using UnityEngine;
using System.Collections;

public enum BossState
{
    Moving,
    Waiting,
    Attacking,
    Dead
}

public class BossBrain : MonoBehaviour
{
    [SerializeField] private BossMovement movement;

    private BossState currentState;

    [SerializeField] private BossAttack attack;

    private Coroutine brainRoutine;

    public void BeginFight()
    {
        brainRoutine = StartCoroutine(BrainLoop());
    }

    public void EndFight()
    {
        StopAllCoroutines();
    }

    private IEnumerator BrainLoop()
    {
        while (true)
        {
            currentState = BossState.Moving;

            movement.MoveToRandomPoint();

            yield return new WaitUntil(() => movement.HasReachedDestination);


            currentState = BossState.Waiting;

            yield return new WaitForSeconds(
                Random.Range(1f, 2f)
            );


            currentState = BossState.Attacking;

            yield return StartCoroutine(
                attack.Execute()
            );
        }
    }
}
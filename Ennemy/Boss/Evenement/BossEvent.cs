using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossEvent : MonoBehaviour
{
    //[Header("Players")]
    //[SerializeField] private PlayerMove player1;
    //[SerializeField] private PlayerMove player2;

    [Header("Arena")]
    [SerializeField] private ArenaController arena;

    [Header("Boss")]
    [SerializeField] private BossBrain bossBrain;
    [SerializeField] private BossIntro bossIntro;
    [SerializeField] private BossHealth bossHealth;

    private bool fightStarted = false;

    private HashSet<GameObject> playersInside = new();

    private void Start()
    {
        // ecoute la mort du boss
        bossHealth.OnBossDeath += HandleBossDeath;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playersInside.Add(other.transform.root.gameObject);

        if (playersInside.Count >= 2 && !fightStarted)
        {
            fightStarted = true;
            StartCoroutine(StartEncounter());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (fightStarted)
            return;

        if (!other.CompareTag("Player"))
            return;

        playersInside.Remove(other.transform.root.gameObject);
    }

    private IEnumerator StartEncounter()
    {
        yield return arena.CloseArena();

        yield return bossIntro.PlayIntro();

        bossBrain.BeginFight();
    }

    private IEnumerator EndEncounter()
    {
        bossBrain.EndFight();

        yield return bossIntro.PlayEnd();

        yield return arena.OpenArena();

        Debug.Log("Combat terminé !");
    }

    private void HandleBossDeath()
    {
        StartCoroutine(EndEncounter());
    }

    private void OnDestroy()
    {
        bossHealth.OnBossDeath -= HandleBossDeath;
    }
}
using UnityEngine;
using System;

public class BossHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;

    public event Action OnBossDeath;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log($"Boss HP : {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        currentHealth = 0;

        OnBossDeath?.Invoke();
    }
}
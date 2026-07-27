using UnityEngine;
using UnityEngine.InputSystem;

// Evenement attack mis dans linspector player input sur le player
public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private Transform attackPoint;

    [Header("FX")]

    [SerializeField] private GameObject[] attackFX;

    [SerializeField] private float radius = 2f;

    [SerializeField] private int damage = 10;

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private Animator animator;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        Attack();
        RandomAttackAnimation();
        SpawnAttackFX();
    }

    private void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(
            attackPoint.position,
            radius,
            enemyLayer
        );

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            attackPoint.position,
            radius
        );
    }

    private void RandomAttackAnimation()
    {
        int attack = Random.Range(0, 5);

        switch (attack)
        {
            case 0:
                animator.SetTrigger("Attack1");
                break;

            case 1:
                animator.SetTrigger("Attack2");
                break;

            case 2:
                animator.SetTrigger("Attack3");
                break;
            case 3:
                animator.SetTrigger("Attack4");
                break;
            case 4:
                animator.SetTrigger("Attack5");
                break;
        }
    }

    private void SpawnAttackFX()
    {
        if (attackFX.Length == 0)
            return;

        GameObject fx = attackFX[Random.Range(0, attackFX.Length)];

        Instantiate(
            fx,
            attackPoint.position,
            attackPoint.rotation
        );
    }
}
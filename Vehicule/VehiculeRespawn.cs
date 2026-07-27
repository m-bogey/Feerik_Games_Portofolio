using UnityEngine;

public class VehiculeRespawn : MonoBehaviour
{
    public GameObject deathFXPrefab;

    private Vector3 spawn;

    private void Start()
    {
        spawn = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KillZone"))
            Die();
        if (other.CompareTag("Respawn"))
            spawn = transform.position;
    }

    private void Die()
    {
        SpawnFX();
        Respawn();
    }

    private void SpawnFX()
    {
        if (deathFXPrefab == null)
            return;

        GameObject fx = Instantiate(deathFXPrefab, transform.position, Quaternion.identity);
        Destroy(fx, 3f);
    }

    private void Respawn()
    {
        transform.position = spawn;
        transform.rotation = Quaternion.identity; // pour remettre une valeur Quaternion.Euler(0f, 0f, 0f);
    }
}

using UnityEngine;
using System.Collections;

public class ExplosionAttack : BossAttack
{
    public float warningTime = 1f;
    public float radius = 10f;

    public GameObject indicatorPrefab;

    [SerializeField] private AttackZone attackZone;

    [SerializeField] private GameObject explosionFX;

    public override IEnumerator Execute()
    {
        Vector3 position = attackZone.GetRandomPoint();

        // Création de la zone rouge
        GameObject obj = Instantiate(indicatorPrefab, position, Quaternion.identity);

        AttackIndicator indicator = obj.GetComponent<AttackIndicator>();

        indicator.Initialize(radius, warningTime);

        yield return new WaitForSeconds(warningTime);

        Destroy(obj);

        SpawnExplosionFX(position);

        Explode(position);
    }

    private void Explode(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(
            position,
            radius
        );


        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("BOOM joueur touché !");
            }
        }
    }

    private void SpawnExplosionFX(Vector3 position)
    {
        GameObject fx = Instantiate(
            explosionFX,
            position,
            Quaternion.identity
        );
        Debug.Log("Le fx a exploser");
        fx.transform.localScale = Vector3.one * radius;

        Debug.Log(fx.activeSelf);
        Debug.Log(fx.activeInHierarchy);

        Destroy(fx, 3f);
    }
}
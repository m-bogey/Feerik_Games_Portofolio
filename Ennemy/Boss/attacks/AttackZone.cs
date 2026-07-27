using UnityEngine;

public class AttackZone : MonoBehaviour
{
    public Vector3 size = new Vector3(20, 0, 20);


    public Vector3 GetRandomPoint()
    {
        float x = Random.Range(-size.x / 2, size.x / 2);
        float z = Random.Range(-size.z / 2, size.z / 2);

        return transform.position + new Vector3(x, 0, z);
    }
}
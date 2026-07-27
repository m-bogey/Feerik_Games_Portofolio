using UnityEngine;
using System.Collections;

public abstract class BossAttack : MonoBehaviour
{
    public string attackName;

    public abstract IEnumerator Execute();
}
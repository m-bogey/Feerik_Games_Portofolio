using UnityEngine;
using System.Collections;

public class ArenaController : MonoBehaviour
{
    [Header("Invisible Walls")]
    [SerializeField] private GameObject invisibleWallsOpen;
    [SerializeField] private GameObject invisibleWallsClose;

    //[Header("Animation")]
    //[SerializeField] private Animator animator;

    public IEnumerator CloseArena()
    {
        invisibleWallsClose.SetActive(true);

        //animator.SetTrigger("Close");

        yield return new WaitForSeconds(2f);
    }

    public IEnumerator OpenArena()
    {
        //animator.SetTrigger("Open");

        yield return new WaitForSeconds(2f);

        invisibleWallsOpen.SetActive(false);
    }
}
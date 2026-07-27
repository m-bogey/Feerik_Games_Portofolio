using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// À mettre sur un objet avec un Collider en mode "Is Trigger".
/// Si un des 2 joueurs entre dans la zone → changement de scène.
///
/// Setup :
///   1. Ajoute ce script + un Collider (IsTrigger = true) sur l'objet
///   2. Remplis "sceneName" dans l'Inspector avec le nom exact de ta scène
///   3. Ta scène doit être ajoutée dans File → Build Settings
///   4. Tes joueurs doivent avoir le tag "Player"
/// </summary>
public class SceneChangeTrigger : MonoBehaviour
{
    [Header("Scene")]
    [Tooltip("Nom exact de la scène dans Build Settings")]
    [SerializeField] private string sceneName;

    [Header("Options")]
    [Tooltip("Délai en secondes avant le changement (0 = immédiat)")]
    [SerializeField] private float delay = 0f;

    [Tooltip("Affiche un effet visuel / son avant de changer")]
    [SerializeField] private GameObject triggerFX;  // optionnel

    private bool _triggered = false;

    // ─────────────────────────────────────────────────────────────────────

    private void OnTriggerEnter(Collider other)
    {
        // Ignore si déjà déclenché ou si ce n'est pas un joueur
        if (_triggered) return;
        if (!other.CompareTag("Player")) return;

        _triggered = true;

        Debug.Log($"[SceneChangeTrigger] Joueur touché ({other.name}) " +
                  $"→ chargement de '{sceneName}' dans {delay}s");

        // Effet visuel / son optionnel
        if (triggerFX != null)
            triggerFX.SetActive(true);

        if (delay <= 0f)
            LoadScene();
        else
            Invoke(nameof(LoadScene), delay);
    }

    private void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("[SceneChangeTrigger] Aucun nom de scène renseigné !");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    // Gizmo pour voir la zone dans l'éditeur
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.8f, 0f, 0.3f);
        Collider col = GetComponent<Collider>();
        if (col is BoxCollider box)
            Gizmos.DrawCube(transform.position, box.size);
        else if (col is SphereCollider sphere)
            Gizmos.DrawSphere(transform.position, sphere.radius);
    }
}
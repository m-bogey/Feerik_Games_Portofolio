using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// À mettre sur chaque joueur placé manuellement dans la scène.
/// Assigne le playerIndex au composant PlayerInput via reflection,
/// avant que PlayerSetup et CinemachinePlayerInput ne le lisent.
///
/// Setup :
///   Player1 = playerIndex = 0
///   Player2 = playerIndex = 1
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class PlayerIndexAssigner : MonoBehaviour
{
    [Tooltip("0 = Joueur 1  /  1 = Joueur 2")]
    [SerializeField] private int playerIndex = 0;

    void Awake()
    {
        PlayerInput pi = GetComponent<PlayerInput>();

        var field = typeof(PlayerInput).GetField(
            "m_PlayerIndex",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance
        );

        if (field != null)
        {
            field.SetValue(pi, playerIndex);
            Debug.Log($"[PlayerIndexAssigner] {gameObject.name} → playerIndex = {playerIndex}");
        }
        else
        {
            Debug.LogError("[PlayerIndexAssigner] Champ m_PlayerIndex introuvable. " +
                           "Vérifie la version du Input System package.");
        }
    }
}